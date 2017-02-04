using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ContentFinder.IoOperation
{
    public class WindowsFileSystemAccessor : IFileSystemAccessor
    {
        public IEnumerable<FileThinInfo> GetFiles( string path, string extension = "*" )
        {
            return Directory.EnumerateFiles( path, $"*.{extension}", SearchOption.AllDirectories )
                            .Select( ConvertToThinInfo );
        }

        private static FileThinInfo ConvertToThinInfo( string path )
        {
            var fileInfo = new FileInfo( path );

            return new FileThinInfo( fileInfo );
        }

        public StreamReader OpenText( string path )
        {
            return File.OpenText( path );
        }
    }
}