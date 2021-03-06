using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ContentFinder;
using ContentFinder.Reading;
using Moq;
using Xunit;

namespace ContentFinderTests.Reading
{
    public class ContentReaderTests
    {
        private readonly Mock<IDateTimeParser> _dateParserMock;

        public ContentReaderTests()
        {
            _dateParserMock = new Mock<IDateTimeParser>();
        }

        [Fact]
        public void ShouldReadAllLinesWithOccurencesOfExpectedText()
        {
            var expectedText = GetTextSeparatedByNewLines( "tricky", "tricky", "tricky" );
            var reader = GetContentReader();

            using ( var streamReader = GetStreamReaderWithContent( expectedText ) )
            {
                var logs = reader.Read( streamReader, "tricky" );

                Assert.Equal( 3, logs.Count() );
            }
        }

        [Fact]
        public void ShouldAppendTextToMatch_UntilLineContainingTerminationStringIsFound()
        {
            const string pattern = "match";
            const string terminator = "no more";
            var content = GetTextSeparatedByNewLines( pattern, "some more content", terminator );
            const string expectedMatchContent = "match\r\nsome more content\r\n";
            var reader = GetContentReader();

            using ( var streamReader = GetStreamReaderWithContent( content ) )
            {
                var log = reader.Read( streamReader, pattern, terminator ).First();

                Assert.Equal( expectedMatchContent, log.Content );
            }
        }

        [Fact]
        public void ShouldParseDate_UsingSpecifiedNumberOfBufferedLines()
        {
            const string date = "2015-01-01";
            const string pattern = "Match";

            var content = GetTextSeparatedByNewLines( date, pattern );
            var reader = GetContentReader();

            using ( var streamReader = GetStreamReaderWithContent( content ) )
            {
                reader.Read( streamReader, pattern ).ToArray();
            }

            _dateParserMock.Verify(
                d => d.TryParse( It.Is<string[]>( c => c[0].Contains( date ) && c[1].Contains( pattern ) ) ) );
        }

        [Fact]
        public void ShouldReportReadingProgressAtBeginingOfFile_Aftereach10Lines_AndAtTheEnd()
        {
            var content = GetTextSeparatedByNewLines( "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10");
            var reader = GetContentReader();
            var reportedProgress = new List<int>();
            reader.FileReadingProgress += ( sender, args ) => reportedProgress.Add( args.Percent );

            using ( var streamReader = GetStreamReaderWithContent( content ) )
            {
                reader.Read( streamReader, "match" ).ToArray();
            }

            Assert.Equal( 2, reportedProgress.Count );
            Assert.Equal( 100, reportedProgress.Last() );
        }

        private ContentReader GetContentReader()
        {
            return new ContentReader( _dateParserMock.Object, 2);
        }

        private static StreamReader GetStreamReaderWithContent( string expectedText )
        {
            var stream = new MemoryStream();

            var writer = new StreamWriter( stream );
            writer.Write( expectedText );
            writer.Flush();
            stream.Seek( 0, SeekOrigin.Begin );

            return new StreamReader( stream );
        }

        private static string GetTextSeparatedByNewLines( params string[] elements )
        {
            var newLine = Environment.NewLine;

            return string.Join( newLine, elements );
        }
    }
}