using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using WindNote.DataAccessLayer.Queries;
using WindNote.Model;

namespace WindNote.DataAccessLayer.Repositories
{
    public class NotesRepository : GenericRepository<Note>
    {
          public NotesRepository(DbContext databaseContext, IGenericQueryFactory genericQueryFactory) 
                                   : base(databaseContext, genericQueryFactory)
          {
          }
    }
}
