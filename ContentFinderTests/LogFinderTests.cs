using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ContentFinder;
using ContentFinder.IoOperation;
using ContentFinder.Reading;
using Moq;
using Xunit;

namespace ContentFinderTests
{
    public class LogFinderTests
    {
        private const string LogsPath = "C:\\Logs\\";
        private const string FileExtension = "txt";
        private const string SearchPattern = "abc";
        private readonly Mock<IFileSystemAccessor> _fsAccessorMock;
        private readonly Mock<IContentReader> _readerMock;
        private readonly LogFinder _finder;

        public LogFinderTests()
        {
            _fsAccessorMock = new Mock<IFileSystemAccessor>();
            _readerMock = new Mock<IContentReader>();
            _finder = new LogFinder( FileExtension, _readerMock.Object, _fsAccessorMock.Object );
        }

        [Fact]
        public void ShouldGetFilesFromPathWithSpecifiedExtension()
        {
            InvokeGetLogs();

            _fsAccessorMock.Verify( f => f.GetFiles( LogsPath, FileExtension, null ) );
        }

        [Fact]
        public void ShouldNotReturnAnyLogs_WhenFileIsOlderThanSpecifiedDate()
        {
            var path = SetupLogAndGetItsPath();

            var logs = _finder.GetLogs( LogsPath, SearchPattern, SearchPattern, DateTime.Now.AddMinutes( 1 ) ).ToArray();

            _fsAccessorMock.Verify( f => f.OpenText( path ), Times.Never );
        }

        [Fact]
        public void ShouldReadContentOfOpenedFile()
        {
            var path = SetupLogAndGetItsPath();
            using ( var textReader = new StreamReader( Stream.Null ) )
            {
                AssignReaderToPath( path, textReader );
                InvokeGetLogs();

                VerifyThatReaderWasUsedToFindLogs( textReader );
            }
        }

        [Fact]
        public void ShouldReadEachFileFromLocation()
        {
            var paths = SetupThreeLogsAndGetPaths();

            InvokeGetLogs();

            VerifyAllFilesWereOpened( paths );
        }

        [Fact]
        public void ShouldReportProgress_WhenFinishedReadingFile()
        {
            SetupLogAndGetItsPath();
            FileProgressEventArgs argsReceived = null;
            _finder.FileQueueProgress += ( sender, args ) => argsReceived = args;

            InvokeGetLogs();

            Assert.Equal(1, argsReceived.FileNumber);
            Assert.Equal(1, argsReceived.TotalFiles);
            Assert.Equal(100, argsReceived.Percent);
            Assert.Equal( "1.txt", argsReceived.FileName );
        }

        [Fact]
        public void ShouldReportFileProgress_WhenReaderReportsIt()
        {
            SetupLogAndGetItsPath();
            var reported = 0;
            _finder.FileReadingProgress += ( sender, args ) => reported = args.Percent;

            _readerMock.Raise( r=>r.FileReadingProgress += null, this, new MatchingProgressEventArgs (10, 1) );

            Assert.Equal( 10, reported );
        }

        private void InvokeGetLogs()
        {
            _finder.GetLogs( LogsPath, SearchPattern ).ToArray();
        }

        private void VerifyThatReaderWasUsedToFindLogs( StreamReader textReader )
        {
            _readerMock.Verify( r => r.Read(SearchPattern, null ) );
        }

        private void AssignReaderToPath( string path, StreamReader textReader )
        {
            _fsAccessorMock.Setup( f => f.OpenText( path ) ).Returns( textReader );
        }

        private string SetupLogAndGetItsPath()
        {
            var file = new FileThinInfo {Path = "C:\\Logs\\1.txt", Name = "1.txt"};
            _fsAccessorMock.Setup( f => f.GetFiles( LogsPath, FileExtension, null ) ).Returns( new List<FileThinInfo> {file} );

            return file.Path;
        }

        private void VerifyAllFilesWereOpened( IReadOnlyList<string> paths )
        {
            VerifyThatFileWasOpened( paths[0] );
            VerifyThatFileWasOpened( paths[1] );
            VerifyThatFileWasOpened( paths[2] );
        }

        private void VerifyThatFileWasOpened( string path )
        {
            _fsAccessorMock.Verify( f => f.OpenText( path ), Times.Once );
        }

        private IReadOnlyList<string> SetupThreeLogsAndGetPaths()
        {
            var files = new[]
            {
                new FileThinInfo {Path = "C:\\Logs\\1.txt"},
                new FileThinInfo {Path = "C:\\Logs\\2.txt"},
                new FileThinInfo {Path = "C:\\Logs\\3.txt"}
            };
            _fsAccessorMock.Setup( f => f.GetFiles( LogsPath, FileExtension, null ) ).Returns( files );

            return files.Select( f => f.Path ).ToArray();
        }
    }
}