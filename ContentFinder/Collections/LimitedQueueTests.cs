using ContentFinder.Reading;
using Xunit;

namespace ContentFinderTests.Reading
{
    public class LimitedQueueTests
    {
        [Fact]
        public void ShouldEnqueueItem()
        { 
            var queue = new LimitedQueue<int>( 1 );

            queue.Enqueue( 1 );

            Assert.Contains( 1, queue );
        }

        [Fact]
        public void ShouldDequeueItem()
        {
            var queue = new LimitedQueue<int>( 1 );
            queue.Enqueue( 1 );

            var item = queue.Dequeue();

            Assert.Equal( 1, item );
        }

        [Fact]
        public void ShouldDequeueOldestItem()
        {
            var queue = new LimitedQueue<int>( 2 );
            queue.Enqueue( 1 );
            queue.Enqueue( 2 );

            var item = queue.Dequeue();

            Assert.Equal( 1, item );
        }

        [Fact]
        public void ShouldAbandonOldItems_WhenQueuingAboveLimt()
        {
            var queue = new LimitedQueue<int>( 1 );
            queue.Enqueue( 1 );

            queue.Enqueue( 2 );
            var item = queue.Dequeue();

            Assert.Equal( 2, item );
        }
    }
}