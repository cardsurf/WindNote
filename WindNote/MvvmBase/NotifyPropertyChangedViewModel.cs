using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using WindNote.MvvmBase;

namespace WindNote.Mvvm
{
    public abstract class NotifyPropertyChangedViewModel : NotifyPropertyChangedObject
    {
        protected IApplicationDispatcher Dispatcher;
        protected IEventAggregator EventAggregator;

        public NotifyPropertyChangedViewModel(IEventAggregator eventAggregator, IApplicationDispatcher dispatcher)
		{
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("EventAggregator is null");
            }
			if (dispatcher == null)
			{
				throw new ArgumentNullException("ApplicationDispatcher is null");
			}

            this.EventAggregator = eventAggregator;
            this.Dispatcher = dispatcher;
		}
    }
}
