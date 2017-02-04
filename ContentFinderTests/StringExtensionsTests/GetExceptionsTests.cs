using ContentFinder.Reading;
using Xunit;

namespace ContentFinderTests.StringExtensionsTests
{
    public class GetExceptionsTests
    {
        [Theory]
        [InlineData( "" )]
        [InlineData( " " )]
        public void WontGetCrazy_WhenEmptyOrWhitespace_AndWontFindAnyExceptions( string content )
        {
            var exceptions = content.GetExceptions();

            Assert.Empty( exceptions );
        }

        [Fact]
        public void ShouldReturnBothExceptions()
        {
            const string content = "Exception and more because there was also NullReferenceException";

            var exceptions = content.GetExceptions();

            Assert.Contains( "Exception", exceptions );
            Assert.Contains( "NullReferenceException", exceptions );
            Assert.Equal( 2, exceptions.Length );
        }

        [Fact]
        public void ShouldReturnEmptyArray_WhenNoExceptionsAreFound()
        {
            const string content = "none of them are here";

            var exceptions = content.GetExceptions();

            Assert.Empty( exceptions );
        }

        [Fact]
        public void ShouldReturnExceptionMixedInText()
        {
            const string content = "blahblah blah InvalidOperationException oh no";

            var exceptions = content.GetExceptions();

            Assert.Contains( "InvalidOperationException", exceptions );
            Assert.Equal( 1, exceptions.Length );
        }

        [Fact]
        public void ShouldReturnSingleException()
        {
            const string content = "IOException";

            var exceptions = content.GetExceptions();

            Assert.Contains( content, exceptions );
            Assert.Equal( 1, exceptions.Length );
        }

        [Fact]
        public void ShouldNotReturnFullyQualifiedExceptionNames()
        {
            const string content = "System.Exception";

            var exceptions = content.GetExceptions();

            Assert.Contains( "Exception", exceptions );
        }
    }
}