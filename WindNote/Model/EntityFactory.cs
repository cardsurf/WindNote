using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindNote.Model
{
    public interface IEntityFactory
    {
        Activity CreateActivity(int idNote);
        Note CreateNote();
    }

    public class EntityFactory : IEntityFactory
    {
        public Activity CreateActivity(int idNote)
        {
            return new Activity(idNote);
        }

        public Note CreateNote()
        {
            return new Note();
        }
    }
}
