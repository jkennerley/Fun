using System.Collections.Generic;

using Fun;
using Fun.Option;

//using System.Collections.Immutable;

//namespace LaYumba.Functional
namespace Fun
{
    using static F;

    public static class DictionaryExt
    {
        public static Option<T> Lookup<K, T>(this IDictionary<K, T> dict, K key)
        {
            T value;
            return dict.TryGetValue(key, out value) ? Some(value) : None;
        }
    }
}
