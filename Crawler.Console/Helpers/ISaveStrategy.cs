namespace Crawler.Console.Helpers;

public interface ISaveStrategy
{
    void Save(IEnumerable<HtmlResult?> htmlResults);
    
}