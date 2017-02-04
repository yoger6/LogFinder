using System;

namespace ContentFinder.Logs
{
    public class Log
    {
        public string Content { get; set; }
        public string Source { get; set; }
        public int Line { get; set; }
        public DateTime? Time { get; set; }
    }
}