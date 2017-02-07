using ContentFinder.IoOperation;

namespace ContentFinder.PowerShell
{
    public class PowershellLogFinder : LogFinder
    {
        private const string Extension = "pslog";

        public PowershellLogFinder( string pattern, string terminator, int lineBuffer = 3 )
            : base( Extension,
                    pattern,
                    terminator,
                    new PowershellContentReaderFactory( lineBuffer ),
                    new WindowsFileSystemAccessor() )
        {
        }
    }
}