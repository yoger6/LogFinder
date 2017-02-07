using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ContentFinder.IoOperation;
using ContentFinder.Logs;
using ContentFinder.Reading;

namespace ContentFinder
{
    public class LogFinder
    {
        private readonly IContentReaderFactory _contentReaderFactory;
        private readonly IFileSystemAccessor _fileSystemAccessor;
        private readonly string _filesExtension;
        protected readonly DateTime DefaultDate;
        private ConcurrentBag<Log> _logs;
        private readonly string _matchPattern;
        private readonly string _terminatorPattern;
        private int _filesRead;
        private int _totalFiles;
        public int ReadThreads = 1;

        public event EventHandler<FileProgressEventArgs> FileQueueProgress;
        public event EventHandler<MatchingProgressEventArgs> FileReadingProgress;

        public LogFinder(
            string filesExtension,
            string matchPattern,
            string terminatorPattern,
            IContentReaderFactory contentReaderFactory,
            IFileSystemAccessor fileSystemAccessor )
        {
            _filesExtension = filesExtension;
            _contentReaderFactory = contentReaderFactory;
            _fileSystemAccessor = fileSystemAccessor;
            _matchPattern = matchPattern;
            _terminatorPattern = terminatorPattern;
            DefaultDate = DateTime.Today;
        }

        public virtual IEnumerable<Log> GetLogs(
            string path,
            DateTime? searchFilesSince = null )
        {
            var files = _fileSystemAccessor.GetFiles( path, _filesExtension, searchFilesSince ).ToArray();
            _totalFiles = files.Length;
            _logs = new ConcurrentBag<Log>();

            files.AsParallel().WithDegreeOfParallelism( ReadThreads ).ForAll( ReadLogs );
           
            return _logs;
        }

        private void ReadLogs( FileThinInfo file )
        {
            using ( var textReader = _fileSystemAccessor.OpenText( file.Path ) )
            {
                using ( var contentReader = _contentReaderFactory.Create( textReader ) )
                {
                    contentReader.FileReadingProgress += OnFileReadingProgress;

                    foreach ( var log in contentReader.Read( _matchPattern, _terminatorPattern ) )
                    {
                        log.Source = file.Path;
                        _logs.Add( log );
                    }
                    var args = new FileProgressEventArgs(file, ++_filesRead, _totalFiles, _logs.Count);
                    OnFileReadingCompleted(args);
                }
            }
        }

        private void OnFileReadingProgress( object sender, MatchingProgressEventArgs args )
        {
            FileReadingProgress?.Invoke(this, args);
        }

        protected virtual void OnFileReadingCompleted( FileProgressEventArgs e )
        {
            FileQueueProgress?.Invoke( this, e );
        }
    }
}