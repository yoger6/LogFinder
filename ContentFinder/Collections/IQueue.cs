using System.Collections;
using System.Collections.Generic;

namespace ContentFinder.Collections
{
    public interface IQueue<T> : ICollection, IReadOnlyCollection<T>
    {
        void Enqueue( T item );
        T Dequeue();
    }
}