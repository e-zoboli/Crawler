namespace Crawler.Console.Utils;

public interface ICsvReader
{
    public IEnumerable<string> ReadCsv(string path);
}