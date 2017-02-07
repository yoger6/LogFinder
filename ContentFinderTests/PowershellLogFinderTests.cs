using System;
using System.Collections.Generic;
using System.Linq;
using ContentFinder;
using ContentFinder.Logs;
using ContentFinder.PowerShell;
using ContentFinderTests.IoOperation;
using Xunit;

namespace ContentFinderTests
{
    public class PowershellLogFinderTests : FileSystemTestBase
    {
        [Fact]
        public void ShouldReturnExpectedLogsFromFile()
        {
            CreateFileWithLogs();
            var finder = new PowershellLogFinder( "git", "\\*{22}" );

            var logs = finder.GetLogs( TestDirectory );

            ValidateLogs( logs.ToArray() );
        }

        private static void ValidateLogs( IReadOnlyList<Log> logs )
        {
            ValidateLog( logs[1], new DateTime( 2017, 01, 21, 08, 27, 11 ), FirstLogContent() );
            ValidateLog( logs[0], new DateTime( 2017, 01, 21, 08, 27, 15 ), SecondLineContent() );
        }

        private static void ValidateLog( Log log, DateTime expectedDate, string expectedContent )
        {
            Assert.Equal( expectedDate, log.Time.Value );
            Assert.Equal( expectedContent, log.Content );
        }

        private static string FirstLogContent()
        {
            return LogFinderTestResources.PowershellFirstLogEntry + Environment.NewLine;
        }

        private static string SecondLineContent()
        {
            return LogFinderTestResources.PowershellSecondLogEntry + Environment.NewLine;
        }

        private void CreateFileWithLogs()
        {
            CreateFileAndGetItsPath( LogFinderTestResources.PowershellLogContent, "pslog" );
        }
    }
}