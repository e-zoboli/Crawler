using Crawler.Console.Helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Crawler.Console;

public class Crawler(
    IHttpClientFactory httpClientFactory,
    ICsvReader csvReader,
    IHostApplicationLifetime hostApplicationLifetime,
    IDataProcessor dataProcessor,
    ILogger<Crawler> logger) : BackgroundService
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
                return dataProcessor.MapToData(url.Url, content);
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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Crawler running at: {time}", DateTimeOffset.Now);
            var parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory())!.FullName;
            var urlsFilePath = Path.Combine(parentDirectory, "app", "data", "urls.csv");
            var contents = await GetContentAsync(urlsFilePath).ConfigureAwait(false);
            
            var results = dataProcessor.ProcessData(contents);

            dataProcessor.SaveResult(results);

            logger.LogInformation("Crawler is stopping");
            hostApplicationLifetime.StopApplication();
        }

    }
}