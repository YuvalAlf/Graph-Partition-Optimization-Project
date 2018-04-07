using System;
using System.Windows.Threading;

namespace Utils.ExtensionMethods
{
    public static class DispatcherExtensions
    {
        public static T Get<T>(this Dispatcher @this, Func<T> getter)
        {
            T t = default(T);
            @this.InvokeAsync(() => t = getter());
            return t;
        }
    }
}
