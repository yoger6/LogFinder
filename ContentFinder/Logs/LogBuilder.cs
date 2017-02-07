using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentFinder.Reading;

namespace ContentFinder.Logs
{
    internal class LogBuilder
    {
        private readonly string[] _buffer;
        private readonly Queue<string> _lines;
        private readonly IDateTimeParser _dateParser;
        private readonly int _lineNumber;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="buffer"> Buffers last line is the one where match occured, some date might be hidden in preceeding lines
        /// like in PowerShell Transcription logs, then there's a need to seek backwards and the buffered lines come handy.</param>
        /// <param name="dateParser">Parser for the date</param>
        /// <param name="lineNumber">Number of the line where match occured</param>
        public LogBuilder( IEnumerable<string> buffer, IDateTimeParser dateParser, int lineNumber )
        {
            _dateParser = dateParser;
            _lineNumber = lineNumber;
            _buffer = buffer.ToArray();
            _lines = new Queue<string>();
        }

        public void Append( string currentLine )
        {
            _lines.Enqueue( currentLine );
        }

        public Log Build()
        {
            var sb = new StringBuilder();
            sb.AppendLine( GetMatchedLine() );

            while ( _lines.Any() )
            {
                var line = _lines.Dequeue();
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