using System.Text.RegularExpressions;

namespace Crawler.Console.Helpers;

public class HtmlProcessor : IHtmlProcessor
{
    private static IEnumerable<string> ExtractLinks(string html)
    {
        const string pattern = @"https?://[^\s/$.?#].[^\s]*";
        Regex regex = new(pattern);
        return regex.Matches(html).Select(m => m.ToString().TrimEnd('"'));
    }

    private static IEnumerable<string> ExtractTitles(string html)
    {
        const string pattern = @"<title>(.*?)</title>";
        Regex regex = new(pattern, RegexOptions.IgnoreCase);
        return regex.Matches(html).Select(m => m.Groups[1].Value);
    }

    public (IEnumerable<string> links, IEnumerable<string> titles) ProcessHtml(string html)
    {
        IEnumerable<string> links = [];
        IEnumerable<string> titles = [];

        Parallel.Invoke(
            () => links = ExtractLinks(html),
            () => titles = ExtractTitles(html)
        );

        return (links, titles);
    }
}