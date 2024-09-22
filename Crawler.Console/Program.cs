﻿using Crawler.Console.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Crawler.Console.Crawler>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ICsvReader, CsvReader>();

using var host = builder.Build();
host.Run();