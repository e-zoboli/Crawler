using Crawler.Console.Helpers;
using FluentAssertions;

namespace Crawler.Test;

public class HtmlProcessorTests
{
    private readonly HtmlProcessor _sut = new();

    [Fact]
    public void ProcessHtml_ReturnCorrectLinksAndTitles()
    {
        //Arrange
        const string htmlString = """
                                  
                                              <html>
                                              <head>
                                                  <title>Example Page</title>
                                              </head>
                                              <body>
                                                  <p>Check out these links:</p>
                                                  <a href="https://www.example.com" title="Example Website">Example</a>
                                                  <a href="http://test.com/path?query=123" title="Test Website">Test</a>
                                                  <a href="https://sub.domain.org" title="Subdomain Website">Subdomain</a>
                                                  <a href="ftp://invalid.url" title="Invalid URL">Invalid</a>
                                              </body>
                                              </html>
                                  """;

        //Act
        var (links, titles) = _sut.ProcessHtml(htmlString);

        //Assert
         links.Should().BeEquivalentTo(new List<string>
        {
            "https://www.example.com",
            "http://test.com/path?query=123",
            "https://sub.domain.org"
        });

        titles.Should().BeEquivalentTo(new List<string>
        {
            "Example Page"
        });
    }
}