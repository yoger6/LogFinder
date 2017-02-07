using System;
using System.IO;
using System.Reflection;

namespace ContentFinder.Reading
{
    public class StreamReaderProgressObserver
    {
        private readonly StreamReader _reader;

        public StreamReaderProgressObserver( StreamReader reader )
        {
            _reader = reader;
        }

        public int Observe()
        {
            return GetReadingProgress( _reader );
        }

        private static int GetReadingProgress( StreamReader reader )
        {
            return (int) ( GetCurrentPosition( reader )/(double) reader.BaseStream.Length*100 );
        }

        private static long GetCurrentPosition( StreamReader reader )
        {
            if ( reader.BaseStream == null ) return 0;

            var type = reader.GetType();
            var charPosition = GetMember( "charPos", reader, type );
            var charLength = GetMember( "charLen", reader, type );

            return reader.BaseStream.Position - charLength + charPosition;
        }

        private static int GetMember( string name, StreamReader reader, Type type )
        {
            return (int) type.InvokeMember( name, Flags, null, reader, null );
        }

        private static BindingFlags Flags => BindingFlags.DeclaredOnly |
                                             BindingFlags.Public |
                                             BindingFlags.NonPublic |
                                             BindingFlags.Instance |
                                             BindingFlags.GetField;
    }
}