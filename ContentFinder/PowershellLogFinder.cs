using ContentFinder.IoOperation;
using ContentFinder.Reading;
using ContentFinder.Reading.DateParsing;

namespace ContentFinder
{
    public class PowershellLogFinder : LogFinder
    {
        private const string Extension = "pslog";

        public PowershellLogFinder()
            : base( Extension, new ContentReader( new PowershellLogDateTimeParser(),null, 3), new WindowsFileSystemAccessor() )
        {
        }
    }
}