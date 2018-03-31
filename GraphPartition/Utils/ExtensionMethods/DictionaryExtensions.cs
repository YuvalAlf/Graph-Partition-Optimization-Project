﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Utils.ExtensionMethods
{
    public static class DictionaryExtensions
    {
        public static K GetKeyOf<K, V>(this Dictionary<K, V> @this, V v, Func<V,V,bool> equality)
        {
            foreach (var k in @this.Keys)
                if (equality(@this[k], v))
                    return k;
            throw new ArgumentException();
        }
    }
}