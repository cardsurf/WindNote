using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using WindNote.Model;
using WindNote.Mvvm;

namespace WindNote.DataAccessLayer.Queries
{
    public interface IGenericQueryFactory
    {
        GenericGetQuery<T> CreateGenericGetQuery<T>(DbContext databaseContext) where T : NotifyPropertyChangedEntity;
        GenericFindQuery<T> CreateGenericFindQuery<T>(DbContext databaseContext) where T : NotifyPropertyChangedEntity;
        GenericInsertQuery<T> CreateGenericInsertQuery<T>(DbContext databaseContext) where T : NotifyPropertyChangedEntity;
        GenericRemoveQuery<T> CreateGenericRemoveQuery<T>(DbContext databaseContext) where T : NotifyPropertyChangedEntity;
        GenericUpdateQuery<T> CreateGenericUpdateQuery<T>(DbContext databaseContext) where T : NotifyPropertyChangedEntity;
    }

    public class GenericQueryFactory : IGenericQueryFactory
    {
        public GenericGetQuery<T> CreateGenericGetQuery<T>(DbContext databaseContext) where T : NotifyPropertyChangedEntity
        {
            return new GenericGetQuery<T>(databaseContext);
        }

        public GenericFindQuery<T> CreateGenericFindQuery<T>(DbContext databaseContext) where T : NotifyPropertyChangedEntity
        {
            return new GenericFindQuery<T>(databaseContext);
        }

        public GenericInsertQuery<T> CreateGenericInsertQuery<T>(DbContext databaseContext) where T : NotifyPropertyChangedEntity
        {
            return new GenericInsertQuery<T>(databaseContext);
        }

        public GenericRemoveQuery<T> CreateGenericRemoveQuery<T>(DbContext databaseContext) where T : NotifyPropertyChangedEntity
        {
            return new GenericRemoveQuery<T>(databaseContext);
        }

        public GenericUpdateQuery<T> CreateGenericUpdateQuery<T>(DbContext databaseContext) where T : NotifyPropertyChangedEntity
        {
            return new GenericUpdateQuery<T>(databaseContext);
        }
    }
}
