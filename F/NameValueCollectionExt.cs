using System.Collections.Specialized;

namespace Ef
{
    public static class NameValueCollectionExt
    {
        public static Option<string> Lookup
            (this NameValueCollection @this, string key)
            => @this[key];
    }
}