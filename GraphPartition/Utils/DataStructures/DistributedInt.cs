using System.Threading;

namespace Utils.DataStructures
{
    public sealed class DistributedInt
    {
        public volatile int Num;

        private DistributedInt(int num) => Num = num;

        public static DistributedInt Init() => new DistributedInt(0);

        public void AddOne() => Interlocked.Increment(ref Num);
        public void MinusOne() => Interlocked.Decrement(ref Num);

        public void WaitForValue(int value)
        {
            while (true)
            {
                if (Num == value)
                    return;
                Thread.Sleep(20);
            }
        }
    }
}
