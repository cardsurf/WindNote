using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using WindNote.Model;
using WindNote.Mvvm;

namespace WindNote.DataAccessLayer.Queries
{

    public class GenericGetQuery<T> : BaseQuery where T : NotifyPropertyChangedEntity
    {
          public GenericGetQuery(DbContext databaseContext): base(databaseContext) 
          {
          }

          public List<T> Execute() 
          {
              DbSet<T> records = this.DatabaseContext.Set<T>();
              return records.ToList<T>();
          }
    }

    public class GenericFindQuery<T> : BaseQuery where T : NotifyPropertyChangedEntity
    {
          public GenericFindQuery(DbContext databaseContext): base(databaseContext) 
          {
          }

          public List<T> Execute(Expression<Func<T, bool>> predicate) 
          {
              IQueryable<T> records = this.DatabaseContext.Set<T>().Where(predicate);
              return records.ToList<T>();
          }
    }

    public class GenericInsertQuery<T> : BaseQuery where T : NotifyPropertyChangedEntity
    {
        public GenericInsertQuery(DbContext databaseContext): base(databaseContext) 
        {
        }

        public T Execute(T entity)
        {
            DbSet<T> records = this.DatabaseContext.Set<T>();
            T result = records.Add(entity);
            return result;
        }
    }

    public class GenericRemoveQuery<T> : BaseQuery where T : NotifyPropertyChangedEntity
    {
        public GenericRemoveQuery(DbContext databaseContext): base(databaseContext)
        {
        }

        public T Execute(T record)
        {
            DbSet<T> records = this.DatabaseContext.Set<T>();
            records.Attach(record);
            T result = records.Remove(record);
            return result;
        }
    }

    public class GenericUpdateQuery<T> : BaseQuery where T : NotifyPropertyChangedEntity
    {
        public GenericUpdateQuery(DbContext databaseContext): base(databaseContext)
        {
        }

        public void Execute(T record)
        {
            List<T> recordsToUpdate = new List<T>() { record };
            this.Execute(recordsToUpdate);
        }

        public void Execute(List<T> recordsToUpdate)
        {
            DbSet<T> records = this.DatabaseContext.Set<T>();
            foreach(T record in recordsToUpdate)
            {
                records.Attach(record);
                this.DatabaseContext.Entry(record).State = EntityState.Modified;
            }
        }

    }

}
