using MetricsAgent.Utils;
using MetricsManager.DAL;
using MetricsManager.MetricsAgentClient;
using NLog.Web;
using Polly;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MetricsManagerTests")]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddNLog("nlog.config");
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IMetricsAgentClient, MetricsAgentClient>()
    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _=> TimeSpan.FromMilliseconds(1000)));

builder.Services.AddEntityFrameworkSqlite()
    .AddDbContext<MetricsDbContext>();

builder.Services.AddAutoMapper(typeof(AutomapperProfile));

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.AddRepositories();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var client = new MetricsDbContext())
{
    client.Database.EnsureCreated();
}

app.Run();
