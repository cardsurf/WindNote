using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WindNote.Mvvm
{
    public abstract class NotifyPropertyChangedObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            if (this.HasAtLeastOneSubsciber())
            {
                this.HandlePropertyChangedEvent(propertyName);
            }   
        }

        private bool HasAtLeastOneSubsciber()
        {
            return this.PropertyChanged != null;
        }

        private void HandlePropertyChangedEvent(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

