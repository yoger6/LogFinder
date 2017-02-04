using System;
using System.Collections.Generic;
using System.Linq;

namespace ContentFinder.IoOperation
{
    public static class FileInfoCollectionExtensions
    {
        public static IEnumerable<FileThinInfo> GetSince( this IEnumerable<FileThinInfo> files, DateTime date )
        {
            return files.Where( f => f.CreationTime >= date );
        }

        public static IEnumerable<FileThinInfo> GetSinceDays( this IEnumerable<FileThinInfo> files, int days )
        {
            var date = DateTime.Today.AddDays( -days );

            return files.GetSince( date );
        }
    }
}