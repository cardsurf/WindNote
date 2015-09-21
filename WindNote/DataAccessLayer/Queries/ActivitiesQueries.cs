using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using WindNote.Model;

namespace WindNote.DataAccessLayer.Queries
{

    public class FindActivitiesIdNoteQuery : BaseQuery
    {
        IGenericQueryFactory GenericQueryFactory;

        public FindActivitiesIdNoteQuery(DbContext databaseContext, IGenericQueryFactory genericQueryFactory)
               : base(databaseContext) 
        {
            this.GenericQueryFactory = genericQueryFactory;
        }

        public List<Activity> Execute(int idNote)
        {
            GenericFindQuery<Activity> query = this.GenericQueryFactory.CreateGenericFindQuery<Activity>(this.DatabaseContext);
            Expression<Func<Activity, bool>> predicate = (activity => activity.IdNote == idNote);
            List<Activity> activities = query.Execute(predicate);
            return activities;
        }
    }

}
