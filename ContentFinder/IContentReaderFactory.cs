using System.IO;
using ContentFinder.Reading;

namespace ContentFinder
{
    public interface IContentReaderFactory
    {
        IContentReader Create(StreamReader streamReader);
    }
}