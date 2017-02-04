using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ContentFinder.Collections;
using ContentFinder.Logs;
using ContentFinder.Reading.DateParsing;
using Timer = System.Timers.Timer;

namespace ContentFinder.Reading
{
    public class ContentReader : IContentReader
    {
        private readonly IDateTimeParser _dateParser;
        private readonly int _bufferSize;

        public event EventHandler<MatchingProgressEventArgs> FileReadingProgress;

        public ContentReader( IDateTimeParser dateParser, int bufferSize = 1 )
        {
            _dateParser = dateParser;
            _bufferSize = bufferSize;
        }

        public IEnumerable<Log> Read( StreamReader reader, string pattern, string terminator = null )
        {
            if ( terminator == null )
            {
                terminator = pattern;
            }

            using ( var progressNotifier = new Timer( 300 ) )
            {
                var progressObserver = new StreamReaderProgressObserver( reader );
                var buffer = new LimitedQueue<string>( _bufferSize );
                var shouldTerminate = false;
                var lineNumber = 0;
                int[] matches = {0};
                progressNotifier.Elapsed += ( sender, args ) =>
                {
                    OnFileReadingProgress( progressObserver.Observe(), matches[0] );
                };
                progressNotifier.Start();
                LogBuilder builder = null;

                while ( reader.Peek() > 0 )
                {
                    var currentLine = reader.ReadLine();
                    buffer.Enqueue( currentLine );
                    lineNumber++;

                    if ( shouldTerminate )
                    {
                        if ( IsTerminator( currentLine, terminator ) )
                        {
                            shouldTerminate = false;
                            yield return builder.Build();
                        }
                        else
                        {
                            builder.Append( currentLine );
                        }
                    }

                    if ( IsMatch( currentLine, pattern ) && !shouldTerminate )
                    {
                        matches[0]++;
                        builder = new LogBuilder( buffer, _dateParser, lineNumber );
                        shouldTerminate = true;
                        if ( reader.Peek() < 1 )
                        {
                            yield return builder.Build();
                        }
                    }
                
                    if (lineNumber == 1 || reader.EndOfStream )
                    {
                        OnFileReadingProgress(progressObserver.Observe(), matches[0]);
                    }
                }
                progressNotifier.Stop();
            }
        }

        private static bool IsTerminator( string line, string pattern )
        {
            return IsMatchSuccess( line, pattern );
        }

        private static bool IsMatch( string line, string pattern)
        {
            return IsMatchSuccess( line, pattern );
        }

        private static bool IsMatchSuccess( string line, string pattern )
        {
            var match = Regex.Match(line, pattern);

            return match.Success;
        }

        protected virtual void OnFileReadingProgress( int percent, int matches )
        {
            FileReadingProgress?.Invoke( this, new MatchingProgressEventArgs(percent,matches));
        }
    }
}