using System;
using System.Threading;

namespace Utils.DataStructures
{
    public sealed class ConcurrentSignal
    {
        private volatile bool signalIsPending;

        public ConcurrentSignal(bool signalIsPending) => this.signalIsPending = signalIsPending;

        public void Signal()
        {
            signalIsPending = true;
        }

        public void WaitForSignalBlocking(TimeSpan time)
        {
            var endingTime = DateTime.Now + time;
            while (!signalIsPending && DateTime.Now < endingTime)
                Thread.Sleep(10);
            signalIsPending = false;
        }

        public bool TryProcessSignal()
        {
            var signal = signalIsPending;
            signalIsPending = false;
            return signal;
        }

    }
}
