using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ContentFinder.IoOperation
{
    public class WindowsFileSystemAccessor : IFileSystemAccessor
    {
        public IEnumerable<FileThinInfo> GetFiles( string path, string extension = "*", DateTime? since = null )
        {
            var directory = new DirectoryInfo( path );

            return directory.EnumerateFiles( SearchPattern( extension ), SearchOption.AllDirectories )
                .Where( IsNotOlderThan( since ) )
                .Select( ConvertToThinInfo );
        }

        private static Func<FileInfo, bool> IsNotOlderThan( DateTime? since )
        {
            return f => !since.HasValue || f.LastWriteTime >= since.Value;
        }

        private static string SearchPattern( string extension )
        {
            return $"*.{extension}";
        }

        private static FileThinInfo ConvertToThinInfo( FileInfo info )
        {
            return new FileThinInfo( info );
        }

        public StreamReader OpenText( string path )
        {
            return File.OpenText( path );
        }
    }
}