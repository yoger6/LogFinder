using System.Collections.Generic;
using System.IO;

namespace ContentFinder.IoOperation
{
    public interface IFileSystemAccessor
    {
        IEnumerable<FileThinInfo> GetFiles( string path, string extension );
        StreamReader OpenText( string path );
    }
}