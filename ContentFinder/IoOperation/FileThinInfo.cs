using System;
using System.IO;

namespace ContentFinder.IoOperation
{
    public class FileThinInfo
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public DateTime LastWriteTime { get; set; }
        public DateTime CreationTime { get; set; }

        public FileThinInfo()
        {
        }

        public FileThinInfo( FileSystemInfo info )
        {
            Name = info.Name;
            Extension = info.Extension;
            Path = info.FullName;
            LastWriteTime = info.LastWriteTime;
            CreationTime = info.CreationTime;
        }
    }
}