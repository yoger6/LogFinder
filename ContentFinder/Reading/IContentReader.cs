using System;
using System.Collections.Generic;
using ContentFinder.Logs;

namespace ContentFinder.Reading
{
    public interface IContentReader : IDisposable
    {
        IEnumerable<Log> Read(string pattern, string terminator = null);

        event EventHandler<MatchingProgressEventArgs> FileReadingProgress;
    }
}