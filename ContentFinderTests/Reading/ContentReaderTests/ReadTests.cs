using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ContentFinder.Reading;
using ContentFinder.Reading.DateParsing;
using Moq;
using Xunit;

namespace ContentFinderTests.Reading.ContentReaderTests
{
    public class ReadTests
    {
        private readonly Mock<IDateTimeParser> _dateParserMock;

        public ReadTests()
        {
            _dateParserMock = new Mock<IDateTimeParser>();
        }

        [Fact]
        public void ShouldReadAllLinesWithOccurencesOfExpectedText()
        {
            var expectedText = GetTextSeparatedByNewLines( "tricky", "tricky", "tricky" );
            
            using ( var streamReader = GetStreamReaderWithContent( expectedText ) )
            {
                var reader = GetContentReader(streamReader);
                var logs = reader.Read("tricky" );

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
            

            using ( var streamReader = GetStreamReaderWithContent( content ) )
            {
                var reader = GetContentReader(streamReader);
                var log = reader.Read(pattern, terminator ).First();

                Assert.Equal( expectedMatchContent, log.Content );
            }
        }

        [Fact]
        public void ShouldParseDate_UsingSpecifiedNumberOfBufferedLines()
        {
            const string date = "2015-01-01";
            const string pattern = "Match";

            var content = GetTextSeparatedByNewLines( date, pattern );
            
            using ( var streamReader = GetStreamReaderWithContent( content ) )
            {
                var reader = GetContentReader(streamReader);
                reader.Read(pattern ).ToArray();
            }

            _dateParserMock.Verify(
                d => d.TryParse( It.Is<string[]>( c => c[0].Contains( date ) && c[1].Contains( pattern ) ) ) );
        }

        [Fact]
        public void ShouldReportReadingProgressAtBeginingOfFile_Aftereach10Lines_AndAtTheEnd()
        {
            var content = GetTextSeparatedByNewLines( "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10");
            var reportedProgress = new List<int>();
            
            using ( var streamReader = GetStreamReaderWithContent( content ) )
            {
                var reader = GetContentReader(streamReader);
                reader.FileReadingProgress += (sender, args) => reportedProgress.Add(args.Percent);
                reader.Read("match" ).ToArray();
            }

            Assert.Equal( 2, reportedProgress.Count );
            Assert.Equal( 100, reportedProgress.Last() );
        }

        private ContentReader GetContentReader(StreamReader reader)
        {
            return new ContentReader( _dateParserMock.Object, reader, 2);
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