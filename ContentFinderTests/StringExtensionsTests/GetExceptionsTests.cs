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

            Assert.Equal(1, exceptions.Length);
            Assert.Contains( "InvalidOperationException", exceptions );
        }

        [Fact]
        public void ShouldReturnSingleException()
        {
            const string content = "IOException";

            var exceptions = content.GetExceptions();

            Assert.Equal(1, exceptions.Length);
            Assert.Contains( content, exceptions );
        }

        [Fact]
        public void ShouldNotReturnFullyQualifiedExceptionNames()
        {
            const string content = "System.Exception";

            var exceptions = content.GetExceptions();

            Assert.Equal(1, exceptions.Length);
            Assert.Contains( "Exception", exceptions );
        }

        [Fact]
        public void ShouldNotReturnExceptionsThatStartWithLowercase()
        {
            const string content = "someException";

            var exceptions = content.GetExceptions();

            Assert.Equal(1, exceptions.Length);
            Assert.Contains("Exception", exceptions);
        }

        [Fact]
        public void ShouldNotIncludeInvalidCharactersWithinExceptionName()
        {
            const string content = "Invalid(Exception";

            var exceptions = content.GetExceptions();

            Assert.Equal(1, exceptions.Length);
            Assert.Contains("Exception", exceptions);
        }

        [Fact]
        public void ShouldReturnOnlyUniqueExceptions()
        {
            const string content = "Exception blah and one more Exception";

            var exceptions = content.GetExceptions();

            Assert.Equal(1, exceptions.Length);
        }

        [Fact]
        public void ShouldNotConsiderExceptionsThatAreFollowedByOtherLowerCaseCharacters()
        {
            const string content = "Exceptions";

            var exceptions = content.GetExceptions();

            Assert.Empty(exceptions);
        }

        [Theory]
        [InlineData("ThrowException")]
        [InlineData("OnException")]
        public void ShouldNotContainFixedContentThatIsNotAnActualException(string content)
        {
            var exceptions = content.GetExceptions();

            Assert.Empty(exceptions);
        }
    }
}