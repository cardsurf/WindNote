using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using WindNote.Model;

namespace WindNote.DataAccessLayer.Context
{
    public class AppUserDatabaseContext : DbContext
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Note> Notes { get; set; }

        public AppUserDatabaseContext(): base("name=AppConfigSqlLiteConnectionString")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        { 
            var sqliteConnectionInitializer =
                new SqliteCreateDatabaseIfNotExists<AppUserDatabaseContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
    }

}
