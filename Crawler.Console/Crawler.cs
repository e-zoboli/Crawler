using Crawler.Console.Helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Crawler.Console;

public class Crawler(IHttpClientFactory httpClientFactory, ICsvReader csvReader, ILogger<Crawler> logger): BackgroundService
{
    private async Task<IEnumerable<string?>> GetContentAsync(string urlsPath)
    {
        var client = httpClientFactory.CreateClient();
        var urls = csvReader.ReadCsv(urlsPath);
        var tasks = urls.Select(async url =>
        {
            try
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (HttpRequestException e)
            {
                logger.LogError(e, "Error message: {ex}",e.Message);
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
            const string urlsPath = "/app/data/urls.csv";
            var contents = await GetContentAsync(urlsPath).ConfigureAwait(false);
            
            await Task.Delay(10000, stoppingToken).ConfigureAwait(false);
        }
        
    }
}