using System;

namespace ContentFinder.Reading
{
    public interface IDateTimeParser
    {
        DateTime? TryParse( string[] lines );
    }
}