using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using WindNote.DataAccessLayer.Context;
using WindNote.DataAccessLayer.Queries;

namespace WindNote.DataAccessLayer.Repositories
{
    public interface IRepositoryFactory
    {
        ActivitiesRepository CreateActivitiesRepository();
        NotesRepository CreateNotesRepository();
    }

    public class RepositoryFactory : IRepositoryFactory
    {
        public ActivitiesRepository CreateActivitiesRepository()
        {
            DbContext databaseContext = new AppUserDatabaseContext();
            IGenericQueryFactory genericQueryFactory = new GenericQueryFactory();
            IActivityQueryFactory activityQueryFactory = new ActivityQueryFactory();
            return new ActivitiesRepository(databaseContext, genericQueryFactory, activityQueryFactory);
        }

        public NotesRepository CreateNotesRepository()
        {
            DbContext databaseContext = new AppUserDatabaseContext();
            IGenericQueryFactory genericQueryFactory = new GenericQueryFactory();
            return new NotesRepository(databaseContext, genericQueryFactory);
        }
    }

}
