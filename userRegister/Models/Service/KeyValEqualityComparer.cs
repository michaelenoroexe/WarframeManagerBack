using System.Diagnostics.CodeAnalysis;

namespace API.Models.Service
{
    internal sealed class KeyValEqualityComparer<K, V> : IEqualityComparer<KeyValuePair<K, V>>
    {
        public bool Equals(KeyValuePair<K, V> x, KeyValuePair<K, V> y)
        {
            return x.GetHashCode() == y.GetHashCode();
        }

        public int GetHashCode([DisallowNull] KeyValuePair<K, V> obj)
        {
            return obj.Key!.GetHashCode();
        }
    }
}
