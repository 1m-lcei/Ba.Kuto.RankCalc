using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddLogging(b =>
    {
        b.ClearProviders();
        b.SetMinimumLevel(LogLevel.Warning);
        b.AddConsole(options =>
        {
            // Generic Host が標準出力にログを流すことでLLM側が通信と誤読してしまうため
            options.LogToStandardErrorThreshold = LogLevel.Trace;
        });
    })
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

var app = builder.Build();
app.Run();
