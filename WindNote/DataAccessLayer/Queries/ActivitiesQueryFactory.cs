using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace WindNote.DataAccessLayer.Queries
{
    public interface IActivityQueryFactory
    {
        FindActivitiesIdNoteQuery CreateFindActivitiesIdNoteQuery(DbContext databaseContext, IGenericQueryFactory genericQueryFactory);
    }

    public class ActivityQueryFactory : IActivityQueryFactory
    {
        public FindActivitiesIdNoteQuery CreateFindActivitiesIdNoteQuery(DbContext databaseContext, IGenericQueryFactory genericQueryFactory)
        {
            return new FindActivitiesIdNoteQuery(databaseContext, genericQueryFactory);
        }
    }
}
