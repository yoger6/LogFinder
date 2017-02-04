using System;
using System.Collections.Generic;
using System.Linq;
using ContentFinder.IoOperation;
using ContentFinder.Logs;
using ContentFinder.Reading;

namespace ContentFinder
{
    public class LogFinder
    {
        private readonly IContentReader _contentReader;
        private readonly string _filesExtension;
        private readonly IFileSystemAccessor _fileSystemAccessor;
        protected readonly DateTime DefaultDate;

        public event EventHandler<FileProgressEventArgs> FileQueueProgress;
        public event EventHandler<MatchingProgressEventArgs> FileReadingProgress; 

        public LogFinder( string filesExtension, IContentReader contentReader, IFileSystemAccessor fileSystemAccessor )
        {
            _filesExtension = filesExtension;
            _contentReader = contentReader;
            _fileSystemAccessor = fileSystemAccessor;

            _contentReader.FileReadingProgress += ( sender, i ) => OnFileReadingProgress( i );
            DefaultDate = DateTime.Today;
        }
        
        //progress and errors 
        public virtual IEnumerable<Log> GetLogs( 
            string path, 
            string matchPattern, 
            string terminatorPattern = null,
            DateTime? searchFilesSince = null )
        {
            var files = _fileSystemAccessor.GetFiles( path, _filesExtension ).ToArray();
            var totalFiles = files.Length;
            var matches = 0;

            for ( var currentFile = 0; currentFile < files.Length; currentFile++ )
            {
                var file = files[currentFile];
                if ( file.CreationTime < searchFilesSince )
                {
                    continue;
                }
                
                var args = new FileProgressEventArgs(file, currentFile + 1, totalFiles, matches);
                OnFileReadingCompleted(args);
                using ( var textReader = _fileSystemAccessor.OpenText( file.Path ) )
                {
                    foreach ( var log in _contentReader.Read( textReader, matchPattern, terminatorPattern ) )
                    {
                        log.Source = file.Path;
                        matches++;
                        yield return log;
                    }
                }

            }
        }

        protected virtual void OnFileReadingCompleted( FileProgressEventArgs e )
        {
            FileQueueProgress?.Invoke( this, e );
        }

        protected virtual void OnFileReadingProgress( MatchingProgressEventArgs e )
        {
            FileReadingProgress?.Invoke( this, e );
        }
    }
}