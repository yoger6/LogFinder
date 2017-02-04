using System;
using System.IO;
using ContentFinder.Reading;
using Xunit;

namespace ContentFinderTests.Reading
{
    public class StreamReaderProgressObserverTests
    {
        [Fact]
        public void ShouldIndicateZeroBeforeReadingFirstLine()
        {
            using ( var reader = GetStreamReaderWithContent( "some text" ) )
            {
                var observer = new StreamReaderProgressObserver( reader );

                Assert.Equal( 0, observer.Observe() );
            }
        }

        [Fact]
        public void ShouldInticate100WhenAllTextIsRead()
        {
            using ( var reader = GetStreamReaderWithContent( "some text" ) )
            {
                var observer = new StreamReaderProgressObserver( reader );

                reader.ReadToEnd();

                Assert.Equal( 100, observer.Observe() );
            }
        }

        [Fact]
        public void ShouldIndicateCorrectValueWhileReadingLines()
        {
            var expectedPercentValues = new[] {0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100};
            var tenLinesContent = GetTenLines();

            using ( var reader = GetStreamReaderWithContent( tenLinesContent ) )
            {
                var observer = new StreamReaderProgressObserver( reader );
                for ( var i = 0; i < expectedPercentValues.Length; i++ )
                {
                    var progress = observer.Observe();
                    VerifyWithTolerance( expectedPercentValues[i], progress, 4 );
                    reader.ReadLine();
                }
            }
        }

        private static string GetTenLines()
        {
            return GetTextSeparatedByNewLines( "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" );
        }

        private static void VerifyWithTolerance( int expected, int actual, int tolerance )
        {
            Assert.True( Math.Abs( expected ) - Math.Abs( actual ) + tolerance > 0 );
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