using System;
using System.Collections.Generic;
using System.IO;

namespace ContentFinder.IoOperation
{
    public interface IFileSystemAccessor
    {
        IEnumerable<FileThinInfo> GetFiles( string path, string extension, DateTime? since = null );
        StreamReader OpenText( string path );
    }
}