using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindNote.DataAccessLayer.Queries;
using WindNote.Mvvm;

namespace WindNote.DataAccessLayer.Repositories
{
    public interface IGenericRepository<T> where T : NotifyPropertyChangedEntity
    {
        List<T> GetAll();
        void Insert(T entity);
        void Remove(T entity);
        void Update(T entity);
        void Update(List<T> entites);
        void CommitChanges();
    }

    public class GenericRepository <T> : IGenericRepository<T> where T : NotifyPropertyChangedEntity
    {
          protected DbContext DatabaseContext;
          protected IGenericQueryFactory GenericQueryFactory;

          public GenericRepository(DbContext databaseContext, IGenericQueryFactory genericQueryFactory)
          {
              this.DatabaseContext = databaseContext;
              this.GenericQueryFactory = genericQueryFactory;
          }

          public virtual List<T> GetAll()
          {
              GenericGetQuery<T> query = this.GenericQueryFactory.CreateGenericGetQuery<T>(this.DatabaseContext);
              List<T> result = query.Execute();
              this.CommitChanges();
              return result;
          }

          public virtual void Insert(T entity)
          {
              GenericInsertQuery<T> query = this.GenericQueryFactory.CreateGenericInsertQuery<T>(this.DatabaseContext);
              T result = query.Execute(entity);
              this.CommitChanges();
          }

          public virtual void Remove(T entity)
          {
              GenericRemoveQuery<T> query = this.GenericQueryFactory.CreateGenericRemoveQuery<T>(this.DatabaseContext);
              T result = query.Execute(entity);
              this.CommitChanges();
          }

          public virtual void Update(T entity)
          {
              GenericUpdateQuery<T> query = this.GenericQueryFactory.CreateGenericUpdateQuery<T>(this.DatabaseContext);
              query.Execute(entity);
              this.CommitChanges();
          }

          public virtual void Update(List<T> entities)
          {
              GenericUpdateQuery<T> query = this.GenericQueryFactory.CreateGenericUpdateQuery<T>(this.DatabaseContext);
              query.Execute(entities);
              this.CommitChanges();
          }

          public virtual void CommitChanges()
          {
              this.DatabaseContext.SaveChanges();
          }

    }
}
