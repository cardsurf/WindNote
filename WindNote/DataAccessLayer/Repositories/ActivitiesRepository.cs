using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using WindNote.DataAccessLayer.Context;
using WindNote.DataAccessLayer.Queries;
using WindNote.Model;

namespace WindNote.DataAccessLayer.Repositories
{

    public class ActivitiesRepository : GenericRepository<Activity>
    {
          private IActivityQueryFactory ActivityQueryFactory;

          public ActivitiesRepository(DbContext databaseContext, IGenericQueryFactory genericQueryFactory,
                                      IActivityQueryFactory activityQueryFactory) : base(databaseContext, genericQueryFactory)
          {
              this.ActivityQueryFactory = activityQueryFactory;
          }

          public List<Activity> FindActivitiesOfNote(int idNote)
          {
              FindActivitiesIdNoteQuery query = ActivityQueryFactory.CreateFindActivitiesIdNoteQuery(this.DatabaseContext, this.GenericQueryFactory);
              List<Activity> result = query.Execute(idNote);
              this.CommitChanges();
              return result;
          }

          public void IncreasePositionsOnListAndInsert(Activity activity)
          {
              this.BeforeInsertIncreasePositionsOnList(activity);
              base.Insert(activity);
          }

          private void BeforeInsertIncreasePositionsOnList(Activity activity)
          {
              int idNote = activity.IdNote;
              FindActivitiesIdNoteQuery getQuery = ActivityQueryFactory.CreateFindActivitiesIdNoteQuery(this.DatabaseContext, this.GenericQueryFactory);
              List<Activity> records = getQuery.Execute(idNote);
              records = this.IncreasePositionsOnList(activity, records);
              GenericUpdateQuery<Activity> query = this.GenericQueryFactory.CreateGenericUpdateQuery<Activity>(this.DatabaseContext);
              query.Execute(records);
          }

          private List<Activity> IncreasePositionsOnList(Activity activity, List<Activity> records)
          {
              foreach (Activity record in records)
              {
                  if (record.PositionOnList >= activity.PositionOnList)
                  {
                      record.PositionOnList += 1;
                  }
              }
              return records;
          }

    }
}
