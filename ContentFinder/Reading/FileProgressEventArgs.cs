using ContentFinder.IoOperation;

namespace ContentFinder.Reading
{
    public class FileProgressEventArgs : MatchingProgressEventArgs
    {
        public int FileNumber { get; }

        public int TotalFiles { get; }

        public string FileName { get; }

        public FileProgressEventArgs( FileThinInfo file, int currentFileNumber, int totalFiles, int matches )
            : base( CalculatePercent( currentFileNumber, totalFiles ), matches )
        {
            FileNumber = currentFileNumber;
            TotalFiles = totalFiles;
            FileName = file.Name;
        }

        private static int CalculatePercent( int currentFileNumber, int totalFiles )
        {
            return (int) ( (double) currentFileNumber/totalFiles*100 );
        }
    }
}