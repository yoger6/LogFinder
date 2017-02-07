using System;
using System.Globalization;
using System.Text.RegularExpressions;
using ContentFinder.Reading;

namespace ContentFinder.PowerShell
{
    internal class PowershellLogDateTimeParser : IDateTimeParser
    {
        public DateTime? TryParse( string[] lines )
        {
            if ( string.IsNullOrWhiteSpace( lines[0] ) )
            {
                return null;
            }

            return ReadTime( lines[0] );
        }

        private static DateTime? ReadTime( string line )
        {
            const string dateFormat = "yyyyMMddHHmmss";

            var datePart = GetDatePart( line );

            if ( string.IsNullOrWhiteSpace( datePart ) )
            {
                return null;
            }

            try
            {
                return DateTime.ParseExact( datePart, dateFormat, CultureInfo.InvariantCulture );
            }
            catch (FormatException)
            {
                return null;
            }
        }

        private static string GetDatePart( string line )
        {
            const string dateRegex = "(20\\d{12})";
            var match = Regex.Match( line, dateRegex );

            return match.Success ? match.Value : null;
        }
    }
}