using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ContentFinder.Reading
{
    public static class ContentParsingExtensions
    {
        public static string[] GetExceptions( this string content )
        {
            return ReadExceptions( content ).ToArray();
        }

        private static IEnumerable<string> ReadExceptions( string content )
        {
            const string exceptionPattern = "([^\\s\\.]*Exception)(?![a-z])";

            var matches = Regex.Matches( content, exceptionPattern );
            
            foreach ( Match match in matches )
            {
                yield return match.Value;
            }
        }
    }
}