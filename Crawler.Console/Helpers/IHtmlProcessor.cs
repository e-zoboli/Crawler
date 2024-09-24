namespace Crawler.Console.Helpers
{
    public interface IHtmlProcessor
    {
        public (IEnumerable<string> links, IEnumerable<string> titles) ProcessHtml(string html);
    }
}