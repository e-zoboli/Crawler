using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Crawler.Console.Helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Crawler.Console;

public class Crawler(IHttpClientFactory httpClientFactory, ICsvReader csvReader, ILogger<Crawler> logger): BackgroundService
{
    private async Task<IEnumerable<HtmlData?>> GetContentAsync(string urlsPath)
    {
        var client = httpClientFactory.CreateClient();
        var urls = csvReader.ReadCsv(urlsPath);
        var tasks = urls.Select(async url =>
        {
            try
            {
                logger.LogInformation("Crawler calling endpoint: {url}", url.Url);
                var response = await client.GetAsync(url.Url);
                response.EnsureSuccessStatusCode();
                logger.LogInformation("Crawler call to {url} successful", url.Url);
                var content = await response.Content.ReadAsStringAsync();
                return MapToData(url.Url, content);
            }
            catch (HttpRequestException e)
            {
                logger.LogError(e, "Crawler call to {url} failed. Error message {ex}", url.Url, e.Message);
                return null;
            }
            
        });
        
        var results = await Task.WhenAll(tasks);
        return results.Where(result => result != null);
    }

    private static HtmlData? MapToData(string url, string content)
    {
        return string.IsNullOrWhiteSpace(content) ? null : new HtmlData(url, content, DateTime.Now);
    }

    private static void WriteToFile(IEnumerable<HtmlData?> htmlData, string filePath)
    {
        var options = new JsonSerializerOptions{WriteIndented = true};
        var json = JsonSerializer.Serialize(htmlData, options);
        File.WriteAllText(filePath, json);
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Crawler running at: {time}", DateTimeOffset.Now);
            string urlsPath = "urls.csv";
            var contents = await GetContentAsync(urlsPath).ConfigureAwait(false);
            var pathToWrite = Directory.GetCurrentDirectory() + "results.json";
            WriteToFile(contents, pathToWrite);
            
            await Task.Delay(10000, stoppingToken).ConfigureAwait(false);
        }
        
    }
}