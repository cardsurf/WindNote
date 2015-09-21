using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace WindNote.MvvmBase
{
    public interface IApplicationDispatcher
    {
        void Invoke(Action action);
        void BeginInvoke(Action action);
    }

    public class ApplicationDispatcher : IApplicationDispatcher
    {
        Dispatcher dispatcher;

        public ApplicationDispatcher()
        {
            this.dispatcher = Application.Current.Dispatcher;
        }
        public void Invoke(Action action)
        {
            this.dispatcher.Invoke(action);
        }

        public void BeginInvoke(Action action)
        {
            this.dispatcher.BeginInvoke(action);
        }
    }
}
