using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Timers;
using ContentFinder.Collections;
using ContentFinder.Logs;
using ContentFinder.Reading.DateParsing;
using Timer = System.Timers.Timer;

namespace ContentFinder.Reading
{
    public class ContentReader : IContentReader
    {
        private readonly IDateTimeParser _dateParser;
        private readonly StreamReader _streamReader;
        private readonly StreamReaderProgressObserver _progressObserver;
        private readonly int _bufferSize;
        private int _matches;

        public event EventHandler<MatchingProgressEventArgs> FileReadingProgress;

        public ContentReader(IDateTimeParser dateParser, StreamReader streamReader, int bufferSize = 1)
        {
            _dateParser = dateParser;
            _streamReader = streamReader;
            _bufferSize = bufferSize;
            _progressObserver = new StreamReaderProgressObserver(_streamReader);
        }

        public IEnumerable<Log> Read(string pattern, string terminator = null)
        {
            if ( terminator == null )
            {
                terminator = pattern;
            }

            using ( var progressNotifier = new Timer( 300 ) )
            {
                var buffer = new LimitedQueue<string>( _bufferSize );
                var shouldTerminate = false;
                var lineNumber = 0;
                progressNotifier.Elapsed += NotifyAboutProgress;
                progressNotifier.Start();
                LogBuilder builder = null;

                while ( _streamReader.Peek() > 0 )
                {
                    var currentLine = _streamReader.ReadLine();
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
                        _matches++;
                        builder = new LogBuilder( buffer, _dateParser, lineNumber );
                        shouldTerminate = true;
                        if (_streamReader.Peek() < 1 )
                        {
                            yield return builder.Build();
                        }
                    }
                
                    if (lineNumber == 1 || _streamReader.EndOfStream )
                    {
                        OnFileReadingProgress(_progressObserver.Observe(), _matches);
                    }
                }
                progressNotifier.Stop();
            }
        }

        private void NotifyAboutProgress(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            OnFileReadingProgress(_progressObserver.Observe(), _matches);
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

        public void Dispose()
        {
            _streamReader.Dispose();
        }
    }
}