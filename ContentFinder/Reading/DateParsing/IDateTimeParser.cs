using System;

namespace ContentFinder.Reading.DateParsing
{
    public interface IDateTimeParser
    {
        DateTime? TryParse( string[] lines );
    }
}