using API.Models;

namespace API.Repositories
{
    public static class Extensions
    {
        public static IEnumerable<LinkedListNode<T>> Nodes<T>(this LinkedList<T> th)
        {
            for (var node = th.First; node != null; node = node.Next)
            {
                yield return node;
            }
        }
    }
}
