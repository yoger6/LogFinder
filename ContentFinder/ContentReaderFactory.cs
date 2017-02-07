using System;
using System.IO;
using ContentFinder.Reading;

namespace ContentFinder
{
    public class ContentReaderFactory : IContentReaderFactory
    {
        private readonly IDateTimeParser _timeParser;
        private readonly int _lineBuffer;

        public ContentReaderFactory( IDateTimeParser timeParser, int lineBuffer )
        {
            _timeParser = timeParser;
            _lineBuffer = lineBuffer;
        }

        public IContentReader Create( StreamReader streamReader )
        {
            if ( streamReader == null )
            {
                throw new ArgumentNullException( nameof( streamReader ) );
            }

            return new ContentReader( _timeParser, streamReader, _lineBuffer );
        }
    }
}