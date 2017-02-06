using System.IO;
using ContentFinder.Reading;
using ContentFinder.Reading.DateParsing;
using Moq;
using Xunit;

namespace ContentFinderTests.Reading.ContentReaderTests
{
    public class ContentReaderTest
    {
        [Fact]
        public void ShouldDisposeUnderlyingStreamReader()
        {
            var streamReader = new StreamReaderSpy(Stream.Null);
            var contentReader = new ContentReader(Mock.Of<IDateTimeParser>(), streamReader);

            contentReader.Dispose();

            Assert.True(streamReader.WasDisposed);
            
        }

        private class StreamReaderSpy : StreamReader
        {
            public bool WasDisposed { get; private set; }

            public StreamReaderSpy(Stream stream) 
                : base(stream)
            {
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                WasDisposed = true;
            }
        }
    }
}