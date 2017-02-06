using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

            var actualFiles = _systemAccessor.GetFiles(TestDirectory);

            AllFilesShouldMatchPaths(paths, actualFiles);
        }

        [Fact]
        public void ShouldFindFilesWithSpecifiedExtension()
        {
            const string expectedExtension = ".txt";

            var files = _systemAccessor.GetFiles(TestDirectory, expectedExtension);

            Assert.All(files, (f) => Assert.Equal(expectedExtension, f.Extension));
        }

        [Fact]
        public void ShouldFindFilesWithSpecificWriteTime()
        {
            CreateFileAndGetPath();
            var acceptDate = DateTime.Now;
            var fileThatShouldBeReturned = CreateFileAndGetPath();

            var files = _systemAccessor.GetFiles(TestDirectory, "*", acceptDate);

            Assert.Equal(1, files.Count());
            Assert.Equal(fileThatShouldBeReturned, files.First().Path);
        }

        private static void AllFilesShouldMatchPaths(IReadOnlyList<string> paths, IEnumerable<FileThinInfo> actualFiles)
        {
            foreach (var file in actualFiles)
            {
                Assert.Contains(file.Path, paths);
            }
        }
    }
}