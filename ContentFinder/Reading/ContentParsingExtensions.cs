using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ContentFinder.Reading
{
    public static class ContentParsingExtensions
    {
        private static readonly string[] BannedNames = {"ThrowException", "OnException"};

        public static string[] GetExceptions( this string content )
        {
            return ReadExceptions( content ).ToArray();
        }

        private static IEnumerable<string> ReadExceptions( string content )
        {
            const string exceptionPattern = "((?![a-z])[^\\s\\.(]*Exception)(?![a-z])";

            var matches = Regex.Matches( content, exceptionPattern );

            return matches.Cast<Match>().Select(m => m.Value).Distinct().Where(IsNotBanned);
        }

        private static bool IsNotBanned(string name)
        {
            return !BannedNames.Contains(name);
        }
    }
}