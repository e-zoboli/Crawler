namespace Crawler.Console.Helpers;

public interface ICsvReader
{
    public IEnumerable<(string Name, string Url)> ReadCsv(string path);
}