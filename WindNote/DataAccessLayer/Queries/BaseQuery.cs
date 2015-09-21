using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace WindNote.DataAccessLayer.Queries
{

    public abstract class BaseQuery
    {
        protected DbContext DatabaseContext;

        public BaseQuery(DbContext databaseContext)
        {
            this.DatabaseContext = databaseContext;
        }
    }
}
