using System.Diagnostics;
using Timer = System.Windows.Forms.Timer;

namespace Nightwrap
{
    internal class SaverTimer : Timer
    {
        const int DEFAULT_INTERVAL = 6000;
        const int MSECONDS_IN_SECOND = 1000;

        public SaverTimer()
        {
            Interval = DEFAULT_INTERVAL;
        }

        public void Set(int timer)
        {
            Interval = timer * MSECONDS_IN_SECOND;
        }

        public void Reset()
        {
            Stop();
            Start();
        }
    }
}
