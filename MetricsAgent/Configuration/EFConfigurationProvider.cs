using MetricsManager.DAL;
using Microsoft.EntityFrameworkCore;

namespace MetricsAgent.Configuration
{
    public class EFConfigurationProvider : ConfigurationProvider
    {
        public EFConfigurationProvider(Action<DbContextOptionsBuilder> optionsAction)
        {
            OptionsAction = optionsAction;
        }

        Action<DbContextOptionsBuilder> OptionsAction { get; }

        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<MetricsDbContext>();

            OptionsAction(builder);

            using (var dbContext = new MetricsDbContext())
            {
                dbContext.Database.EnsureCreated();

                Data = !dbContext.Settings.Any()
                    ? CreateAndSaveDefaultValues(dbContext)
                    : dbContext.Settings.ToDictionary(c => c.Id, c => c.Value);
            }
        }

        public override void Set(string key, string value)
        {
            base.Set(key, value);

            using (var dbContext = new MetricsDbContext())
            {
                dbContext.Database.EnsureCreated();

                var firstRow = dbContext.Settings.First();
                firstRow.Value = value;

                dbContext.SaveChanges();
            }
        }

        private static IDictionary<string, string> CreateAndSaveDefaultValues(
            MetricsDbContext dbContext)
        {
            var configValues =
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                { "Enabled", bool.TrueString },
                };

            dbContext.Settings.AddRange(configValues
                .Select(kvp => new EFConfigurationValue
                {
                    Id = kvp.Key,
                    Value = kvp.Value
                })
                .ToArray());

            dbContext.SaveChanges();

            return configValues;
        }
    }
}
