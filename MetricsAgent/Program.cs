using MetricsManager.DAL;
using NLog.Web;
using MetricsAgent.Utils;
using MetricsAgent.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEntityFrameworkSqlite()
    .AddDbContext<MetricsDbContext>();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddNLog("nlog.config");
});

builder.Services.AddAutoMapper(typeof(AutomapperProfile));

builder.Services.AddConfig(builder.Configuration);
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.AddRepositories();

builder.WebHost.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddEFConfiguration(
        options => options.UseSqlite("Filename=Metrics.db"));
});

builder.Services.AddScheduledJobs(builder.Configuration);

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

app.Run();