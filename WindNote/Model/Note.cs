using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using WindNote.Mvvm;

namespace WindNote.Model
{
    [Table("Note")]
    public class Note : NotifyPropertyChangedEntity
    {
        [Key]
        public int Id { get; set; }
        private String _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                this.RaisePropertyChangedEvent("Title");
            }
        }
        public int PositionOnList { get; set; }

        [NotMapped]
        public String TitleMenuItem
        {
            get
            {
                return (this.Title == null || this.Title.Length == 0) ? "<Untitled>" : this.Title;
            }
        }
    }
}
