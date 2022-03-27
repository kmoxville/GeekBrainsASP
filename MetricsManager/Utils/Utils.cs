using FluentMigrator.Runner;
using MetricsAgent.Config;
using MetricsManager.DAL;
using NLog.Web;

namespace MetricsAgent.Utils
{
    public static class Utils
    {
        public static IServiceCollection AddRepositories(
             this IServiceCollection services)
        {
            services.AddScoped<ICpuRepository, CpuRepository>();
            services.AddScoped<IAgentsRepository, AgentsRepository>();

            return services;
        }

        public static IServiceCollection ConfigureDatabase(
             this IServiceCollection services, IConfiguration config)
        {
            var dbOptions = config.GetSection(DbOptions.Name).Get<DbOptions>();

            using (var client = new MetricsDbContext())
            {
                client.Database.EnsureCreated();
            }

            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSQLite()
                    .WithGlobalConnectionString($"Data Source={dbOptions.PathToFile}")
                    .ScanIn(typeof(Utils).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddNLog("nlog.config"));

            var serviceProvider = services.BuildServiceProvider();
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            runner.MigrateUp();

            return services;
        }
    }
}
