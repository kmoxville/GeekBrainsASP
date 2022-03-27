using System;

namespace MetricsAgent.Config
{
    public class DbOptions
    {
        public static string Name = "Database";

        public string PathToFile { get; set; } = string.Empty;
    }

    public class BackgroundJobOptions
    {
        public static string Name = "BackgroundJob";

        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(5);
    }
}
