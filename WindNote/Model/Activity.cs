using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using WindNote.Mvvm;

namespace WindNote.Model
{
    [Table("Activity")]
    public class Activity : NotifyPropertyChangedEntity
    {
        public Activity()
        {}

        public Activity(int idNote)
        {
            this.IdNote = idNote;
        }

        [Key]
        public int Id { get; set; }
        public int IdNote { get; set; }
        private String _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.RaisePropertyChangedEvent("Name");
            }
        }
        public int PositionOnList { get; set; }

        [ForeignKey("IdNote")]
        public Note Note { get; set; }
    }
}
