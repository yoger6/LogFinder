using System;

namespace ContentFinder.Reading
{
    public class MatchingProgressEventArgs : EventArgs
    {
        public int Percent { get; set; }
        public int Matches { get; set; }

        public MatchingProgressEventArgs( int percent, int matches )
        {
            Percent = percent;
            Matches = matches;
        }
    }
}