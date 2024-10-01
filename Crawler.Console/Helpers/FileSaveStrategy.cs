using System.Text.Json;

namespace Crawler.Console.Helpers;

public class FileSaveStrategy : ISaveStrategy
{
    public void Save(IEnumerable<HtmlResult?> htmlResults)
    {
        var parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory())!.FullName;
        var writingPath = Path.Combine(parentDirectory, "app","data", "results.json");
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(htmlResults, options);
        File.WriteAllText(writingPath, json);
    }
}