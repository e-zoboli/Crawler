namespace Crawler.Console;

public record HtmlResult(string Url, IEnumerable<string> Links, IEnumerable<string> Titles, DateTime RetrievedAt);