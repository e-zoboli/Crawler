
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.Console.Helpers;

public class DataProcessor(IHtmlProcessor htmlProcessor, [FromKeyedServices("file")]ISaveStrategy saveStrategy) : IDataProcessor
{

    public IEnumerable<HtmlResult?> ProcessData(IEnumerable<HtmlData?> contents)
    {
        IEnumerable<HtmlResult?> listOfResult = [];
        foreach (var htmlData in contents)
        {
            var (links, titles) = htmlProcessor.ProcessHtml(htmlData!.HtmlContent);
            var htmlProcessingResult = MapToResult(htmlData.Url, links, titles, htmlData.RetrievedAt);
            listOfResult = listOfResult.Append(htmlProcessingResult);
        }
        
        return listOfResult;
    }

    public HtmlResult? MapToResult(string url, IEnumerable<string> links, IEnumerable<string> titles, DateTime retrievedAt)
    {
        return new HtmlResult(url, links, titles, retrievedAt);
    }

    public HtmlData? MapToData(string url, string content)
    {
        return string.IsNullOrWhiteSpace(content) ? null : new HtmlData(url, DateTime.Now, content);
    }

    public void SaveResult(IEnumerable<HtmlResult?> htmlResults)
    {
        saveStrategy.Save(htmlResults);
    }
}