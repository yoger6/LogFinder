using System;
using System.Collections.Generic;
using System.Linq;
using ContentFinder.IoOperation;
using Xunit;

namespace ContentFinderTests.IoOperation
{
    public class FileInfoCollectionExtensionsTests : FileSystemTestBase
    {
        private readonly IEnumerable<FileThinInfo> _allFiles;

        public FileInfoCollectionExtensionsTests()
        {
            _allFiles = new[]
            {
                new FileThinInfo { CreationTime = DateTime.Today},
                new FileThinInfo { CreationTime = DateTime.Today.AddDays( -1 )},
                new FileThinInfo { CreationTime = DateTime.Today.AddDays( -2 )}
            };
        }

        [Fact]
        public void ShouldReturnFilesNotOlderThanSpecified()
        {
            var date = DateTime.Today.AddDays( -1 );

            var filteredFiles = _allFiles.GetSince( date ).ToArray();

            Assert.Equal( 2, filteredFiles.Length );
        }

        [Fact]
        public void ShouldReturnFilesSinceDays()
        {
            var filteredFiles = _allFiles.GetSinceDays( 1 );

            Assert.Equal( 2, filteredFiles.Count() );
        }
    }
}