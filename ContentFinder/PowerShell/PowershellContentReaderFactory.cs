namespace ContentFinder.PowerShell
{
    internal class PowershellContentReaderFactory : ContentReaderFactory
    {
        public PowershellContentReaderFactory( int lineBuffer ) 
            : base( new PowershellLogDateTimeParser(), lineBuffer )
        {
        }
    }
}