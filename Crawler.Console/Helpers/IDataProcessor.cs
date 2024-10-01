namespace Crawler.Console.Helpers;

public interface IDataProcessor
{
    public IEnumerable<HtmlResult?> ProcessData(IEnumerable<HtmlData?> data);
    public HtmlData? MapToData(string url, string content);
    public HtmlResult? MapToResult(string url, IEnumerable<string> links, IEnumerable<string> titles, DateTime retrievedAt);
    public void SaveResult(IEnumerable<HtmlResult?> htmlResults);
}