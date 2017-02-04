using System;
using System.Collections.Generic;
using System.IO;
using ContentFinder.Logs;

namespace ContentFinder.Reading
{
    public interface IContentReader
    {
        IEnumerable<Log> Read( StreamReader reader, string pattern, string terminator = null );
        event EventHandler<MatchingProgressEventArgs> FileReadingProgress;
    }
}