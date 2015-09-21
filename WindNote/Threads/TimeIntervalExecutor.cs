using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace WindNote.Threads.TimeIntervalExecutor
{
    public interface ITimeIntervalExecutor
    {
        void Start();
        void Stop();
    }

    class TimeIntervalExecutor : DispatcherTimer, ITimeIntervalExecutor
    {
        public TimeIntervalExecutor(EventHandler method, int updateEverySeconds)
        {
            this.Tick += new EventHandler(method);
            this.Interval = new TimeSpan(0, 0, updateEverySeconds);
        }
    }
}
