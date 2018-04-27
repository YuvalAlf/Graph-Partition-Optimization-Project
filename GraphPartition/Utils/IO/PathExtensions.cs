using System.IO;

namespace Utils.IO
{
    public static class PathExtensions
    {
        public static string CombinePathWith(this string @this, string toCombine) => Path.Combine(@this, toCombine);
    }
}
