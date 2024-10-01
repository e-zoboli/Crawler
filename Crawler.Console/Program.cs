using Crawler.Console.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Crawler.Console.Crawler>();
builder.Services.AddHttpClient();
builder.Services.AddTransient<ICsvReader, CsvReader>();
builder.Services.AddTransient<IHtmlProcessor, HtmlProcessor>();
builder.Services.AddTransient<IDataProcessor, DataProcessor>();
builder.Services.AddKeyedTransient<ISaveStrategy, FileSaveStrategy>("file");

using var host = builder.Build();
host.Run();