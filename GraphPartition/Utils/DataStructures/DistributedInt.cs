using System.Threading;

namespace Utils.DataStructures
{
    public sealed class DistributedInt
    {
        public volatile int Num;

        public DistributedInt(int num)
        {
            Num = num;
        }

        public static DistributedInt Init() => new DistributedInt(0);

        public void AddOne() => Interlocked.Add(ref Num, 1);
        public void MinusOne() => Interlocked.Add(ref Num, -1);

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
