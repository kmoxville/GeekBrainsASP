using Microsoft.EntityFrameworkCore;

namespace Lesson1.Models
{
    public class TemperatureStampsContext : DbContext
    {
        public TemperatureStampsContext(DbContextOptions opt)
            : base(opt)
        {

        }

        public DbSet<TemperatureStamp> TemperatureStamps { get; set; } = null!;
    }
}
