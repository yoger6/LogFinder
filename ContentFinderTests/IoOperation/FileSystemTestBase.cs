using System;
using System.IO;
using System.Reflection;

namespace ContentFinderTests.IoOperation
{
    public class FileSystemTestBase : IDisposable
    {
        public string TestDirectory { get; private set; }

        public FileSystemTestBase()
        {
            CreateRootDirectory();
        }

        private void CreateRootDirectory()
        {
            var directoryLocation = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );
            var directoryName = Guid.NewGuid().ToString();
            TestDirectory = Path.Combine( directoryLocation, directoryName );

            Directory.CreateDirectory( TestDirectory );
        }

        public void Dispose()
        {
            var files = Directory.GetFiles( TestDirectory, "*.*", SearchOption.AllDirectories );

            foreach (var file in files)
            {
                File.Delete(file);
            }
            Directory.Delete( TestDirectory );
        }

        protected string CreateFileAndGetPath()
        {
            var path = GetFile("txt");
            CreateFile(path);
            return path;
        }

        protected string[] CreateFilesAndGetPaths()
        {
            var paths = new[]
            {
                GetFile( "txt" ),
                GetFile( "png" ),
            };

            foreach (var path in paths)
            {
                CreateFile(path);
            }

            return paths;
        }

        private static void CreateFile( string path )
        {
            File.Create( path ).Close();
        }

        private string GetFile( string extension )
        {
            return Path.Combine( TestDirectory, $"{Guid.NewGuid()}.{extension}" );
        }

        protected string CreateFileAndGetItsPath( string content, string extension = "txt" )
        {
            var file = GetFile( extension );
            using ( var writer = File.CreateText( file ) )
            {
                writer.Write( content );
            }

            return file;
        }
    }
}