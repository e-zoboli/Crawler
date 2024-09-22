namespace Crawler.Console.Helpers;

public class CsvReader : ICsvReader
{
    public IEnumerable<(string Name, string Url)> ReadCsv(string path)
    {
        return File.ReadAllLines(path)
            .Skip(1)
            .Select(line => line.Split(','))
            .Where(values => values.Length == 2)
            .Select(values => (Name: values[0].Trim(), Url: values[1].Trim()));
    }
    
}