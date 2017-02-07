using System;
using System.IO;
using ContentFinder;
using ContentFinder.Reading;
using Moq;
using Xunit;

namespace ContentFinderTests
{
    public class ContentReaderFactoryTests
    {
        private readonly ContentReaderFactory _factory;

        public ContentReaderFactoryTests()
        {
            _factory = new ContentReaderFactory(Mock.Of<IDateTimeParser>(), 0);
        }

        [Fact]
        public void ShouldThrow_WhenTextReaderIsNull()
        {
            var create = new Action(() => _factory.Create(null));

            Assert.Throws<ArgumentNullException>(create);
        }

        [Fact]
        public void ShouldReturnContentReader()
        {
            using (var streamReader = new StreamReader(Stream.Null))
            {
                var contentReader = _factory.Create(streamReader);

                Assert.NotNull(contentReader);
            }
        }
    }
}
