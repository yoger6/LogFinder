using System.Collections.Generic;
using ContentFinder.IoOperation;
using Xunit;

namespace ContentFinderTests.IoOperation.WindowsFileSystemAccessorTests
{
    public class GetFilesTests : FileSystemTestBase
    {
        private readonly WindowsFileSystemAccessor _systemAccessor;

        public GetFilesTests()
        {
            _systemAccessor = new WindowsFileSystemAccessor();
        }

        [Fact]
        public void ShouldFindFilesWithinFolder()
        {
            var paths = CreateFilesAndGetPaths();

            var actualFiles = _systemAccessor.GetFiles( TestDirectory );

            AllFilesShouldMatchPaths( paths, actualFiles );
        }

        [Fact]
        public void ShouldFindFilesWithSpecifiedExtension()
        {
            const string expectedExtension = ".txt";

            var files = _systemAccessor.GetFiles( TestDirectory, expectedExtension );

            Assert.All( files, ( f ) => Assert.Equal( expectedExtension, f.Extension ) );
        }

        private static void AllFilesShouldMatchPaths( IReadOnlyList<string> paths, IEnumerable<FileThinInfo> actualFiles )
        {
            foreach (var file in actualFiles)
            {
                Assert.Contains(file.Path, paths);
            }
        }
    }
}