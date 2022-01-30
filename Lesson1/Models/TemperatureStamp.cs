using Microsoft.EntityFrameworkCore;

namespace Lesson1.Models
{
    public class TemperatureStamp
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public decimal TemperatureC { get; set; }

        public decimal TemperatureF => TemperatureC + (decimal)273.15;
    }
}
