namespace Crawler.Console.Helpers;

public class CsvReader : ICsvReader
{
    public IEnumerable<string> ReadCsv(string path)
    {
        return File.ReadAllLines(path)
            .SelectMany(l => l.Split(','))
            .Select(l => l.Trim());

    }
    
}