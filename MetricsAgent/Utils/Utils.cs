using FluentMigrator.Runner;
using MetricsAgent.Config;
using MetricsAgent.DAL.Migrations;
using MetricsManager.DAL;
using MetricsManager.Jobs;
using NLog.Web;
using Quartz;

namespace MetricsAgent.Utils
{
    public static class Utils
    {
        public static IServiceCollection AddConfig(
             this IServiceCollection services, IConfiguration config)
        {
            services.Configure<DbOptions>(
                config.GetSection(DbOptions.Name));

            services.Configure<BackgroundJobOptions>(
                config.GetSection(BackgroundJobOptions.Name));

            return services;
        }

        public static IServiceCollection AddScheduledJobs(
             this IServiceCollection services, IConfiguration config)
        {
            var backgroundJobOptions = config.GetSection(BackgroundJobOptions.Name).Get<BackgroundJobOptions>();
            var interval = (int)(backgroundJobOptions?.Interval.TotalSeconds ?? 5);
            bool enabled = bool.Parse(config["Enabled"]);
            
            services.AddScoped<CpuMetricJob>()
                .AddQuartz(cfg =>
            {
                cfg.UseMicrosoftDependencyInjectionJobFactory();
                cfg.SchedulerName = "default";
                cfg.ScheduleJob<CpuMetricJob>(trigger =>
                {
                    trigger.WithIdentity("cpu-metric", "metrics")
                    .WithDailyTimeIntervalSchedule(sch => sch.WithInterval(interval, IntervalUnit.Second));

                    if (enabled)
                        trigger.StartNow();
                });
            });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            return services;
        }

        public static IServiceCollection AddRepositories(
             this IServiceCollection services)
        {
            services.AddScoped<ICpuRepository, CpuRepository>();

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
                    .ScanIn(typeof(DropAgentsInfos).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddNLog("nlog.config"));

            var serviceProvider = services.BuildServiceProvider();
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            runner.MigrateUp();

            return services;
        }
    }
}
