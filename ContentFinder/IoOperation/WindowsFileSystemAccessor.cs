using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ContentFinder.IoOperation
{
    public class WindowsFileSystemAccessor : IFileSystemAccessor
    {
        public IEnumerable<FileThinInfo> GetFiles(string path, string extension = "*", DateTime? since = null)
        {
            var dir = new DirectoryInfo(path);
            if (since.HasValue)
            {
                return dir.EnumerateFiles($"*.{extension}", SearchOption.AllDirectories)
                    .OrderBy(f => f.LastWriteTime)
                    .Where(f => f.LastWriteTime >= since.Value)
                    .Select(ConvertToThinInfo);
            }
            return dir.EnumerateFiles($"*.{extension}", SearchOption.AllDirectories)
                .Select(ConvertToThinInfo);
        }

        private static FileThinInfo ConvertToThinInfo(FileInfo info)
        {
            return new FileThinInfo(info);
        }

        public StreamReader OpenText(string path)
        {
            return File.OpenText(path);
        }
    }
}