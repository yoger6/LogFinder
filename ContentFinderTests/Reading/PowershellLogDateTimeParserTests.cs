using System;
using ContentFinder.Reading;
using ContentFinder.Reading.DateParsing;
using Xunit;

namespace ContentFinderTests.Reading
{
    public class PowershellLogDateTimeParserTests
    {
        private readonly PowershellLogDateTimeParser _parser;

        public PowershellLogDateTimeParserTests()
        {
            _parser = new PowershellLogDateTimeParser();
        }

        [Theory]
        [InlineData("s")]
        [InlineData(" ")]
        [InlineData("No date here mate")]
        public void ShouldReturnNull_WhenLineIsNullWhitespaceOrDoesntContainDate(string lastLineContent)
        {
            var lines = new [] {lastLineContent};

            var date = _parser.TryParse( lines );

            Assert.Null( date );
        }

        [Fact]
        public void ShouldParseTime_WhenInSpecifiedFormat_AndLocatedInFirstLine()
        {
            const string powershellTime = "20170121082711";
            var lines = new[] {powershellTime, "probably just stars", "here will go the match"};

            var date = _parser.TryParse( lines );

            Assert.Equal( new DateTime(2017, 1, 21, 8, 27, 11), date );
        }

        [Fact]
        public void ShouldReturnNull_WhenTimeCannotBeParsed()
        {
            const string invalidPowershellTime = "20170132082711";
            var lines = new[] {invalidPowershellTime};

            var date = _parser.TryParse( lines );

            Assert.Null( date );
        }

        [Fact]
        public void ShouldParseTime_IfThereArePreceedingCharacters()
        {
            const string timeWithSomeOtherText = "The time is: 20170121082711";
            var lines = new[] {timeWithSomeOtherText};

            var date = _parser.TryParse( lines );

            Assert.Equal(new DateTime(2017, 1, 21, 8, 27, 11), date);
        }
    }
}
