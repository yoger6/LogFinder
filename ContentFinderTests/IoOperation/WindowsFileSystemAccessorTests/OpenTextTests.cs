using System.IO;
using ContentFinder.IoOperation;
using Xunit;

namespace ContentFinderTests.IoOperation.WindowsFileSystemAccessorTests
{
    public class OpenTextTests : FileSystemTestBase
    {
        private readonly WindowsFileSystemAccessor _accessor;

        public OpenTextTests()
        {
            _accessor = new WindowsFileSystemAccessor();
        }

        [Fact]
        public void ShouldOpenFileAndBeAbleToReadItsContent()
        {
            const string expectedContent = "unbelivable!";
            var file = CreateFileAndGetItsPath(expectedContent);
            
            MakeSureStreamConsistsOfContent(file, expectedContent);
        }

        [Fact]
        public void ShouldThrowIoException_WhenCannotOpenFile()
        {
            const string nonExistingFile = "C:\\imagination.png";

            Assert.ThrowsAny<IOException>( ()=> _accessor.OpenText( nonExistingFile ) );
        }

        private void MakeSureStreamConsistsOfContent(string path, string expectedContent)
        {
            using (var stream = _accessor.OpenText(path))
            {
                var actual = stream.ReadToEnd();

                Assert.Equal(expectedContent, actual);
            }
        }
    }
}
