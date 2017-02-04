using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentFinder.Reading;
using ContentFinder.Reading.DateParsing;

namespace ContentFinder.Logs
{
    internal class LogBuilder
    {
        private readonly IDateTimeParser _dateParser;
        private readonly int _lineNumber;
        private readonly string[] _buffer;
        private readonly Queue<string> _addedLines;

        public LogBuilder( IEnumerable<string> buffer, IDateTimeParser dateParser, int lineNumber )
        {
            _dateParser = dateParser;
            _lineNumber = lineNumber;
            _buffer = buffer.ToArray();
            _addedLines = new Queue<string>();
        }

        public void Append( string currentLine )
        {
            _addedLines.Enqueue( currentLine );
        }

        public Log Build()
        {
            var sb = new StringBuilder();
            sb.AppendLine( GetMatchedLine() );
            while ( _addedLines.Any() )
            {
                var line = _addedLines.Dequeue();
                sb.AppendLine( line );
            }

            return GetLog( sb.ToString() );
        }

        private Log GetLog( string content )
        {
            var log = new Log
            {
                Content = content,
                Time = _dateParser.TryParse( _buffer ),
                Line = _lineNumber
            };

            return log;
        }

        private string GetMatchedLine()
        {
            return _buffer.Last();
        }
    }
}