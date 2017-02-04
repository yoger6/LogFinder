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
            return GetReadingProgress(_reader);
        }

        private static int GetReadingProgress(StreamReader reader)
        {
            return (int)(GetCurrentPosition( reader ) / (double)reader.BaseStream.Length*100);
        }

        private static long GetCurrentPosition( StreamReader reader )
        {
            var type = reader.GetType();
            var charPos = (int)type.InvokeMember("charPos",
                BindingFlags.DeclaredOnly |
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.GetField
                , null, reader, null);
            var charlen = (int)type.InvokeMember("charLen",
                BindingFlags.DeclaredOnly |
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.GetField
                , null, reader, null);

            return reader.BaseStream.Position - charlen + charPos;
        }
    }
}