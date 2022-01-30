using MetricsManager.DAL;
using MetricsManager.Models;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.MapType<TimeSpan>(() => new OpenApiSchema
{
    Type = "string",
    Example = new OpenApiString("00:00:00")
}));

builder.Services.AddEntityFrameworkSqlite()
    .AddDbContext<MetricsDbContext>();

builder.Services.AddScoped<ICpuRepository, CpuRepository>();
builder.Services.AddScoped<IHddRepository, HddRepository>();
builder.Services.AddScoped<INetworkRepository, NetworkRepository>();
builder.Services.AddScoped<IRamRepository, RamRepository>();

builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddNLog("nlog.config");
    });


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
