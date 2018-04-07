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
            if (signalIsPending)
                throw new Exception();
            signalIsPending = true;
        }

        public void WaitForSignalBlocking()
        {
            while (!signalIsPending)
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
