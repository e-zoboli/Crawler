namespace Crawler.Console.Helpers;

public interface ICsvReader
{
    public IEnumerable<string> ReadCsv(string path);
}