using System;

namespace Utils.Option
{
    public sealed class NoneOptionException : Exception
    {
        public NoneOptionException(string message) : base(message)
        {
        }
    }
}
