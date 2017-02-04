using System;
using System.Collections;
using System.Collections.Generic;

namespace ContentFinder.Collections
{
    public class LimitedQueue<T> : IQueue<T>
    {
        private readonly int _capacity;
        private readonly Queue<T> _queue;

        public LimitedQueue( int capacity )
        {
            _capacity = capacity;
            _queue = new Queue<T>( capacity );
        }

        public void Enqueue( T item )
        {
            if ( IsQueueAtMaximumCapacity() )
            {
                _queue.Dequeue();
            }

            _queue.Enqueue( item );
        }

        public T Dequeue()
        {
            return _queue.Dequeue();
        }

        private bool IsQueueAtMaximumCapacity()
        {
            return _queue.Count == _capacity;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection.CopyTo( Array array, int index )
        {
            ( (ICollection) _queue ).CopyTo( array, index );
        }

        int ICollection.Count => _queue.Count;

        object ICollection.SyncRoot => ( (ICollection) _queue ).SyncRoot;

        bool ICollection.IsSynchronized => ( (ICollection) _queue ).IsSynchronized;

        int IReadOnlyCollection<T>.Count => _queue.Count;
    }
}