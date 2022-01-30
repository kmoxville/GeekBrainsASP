#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lesson1.Models;

namespace Lesson1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureStampsController : ControllerBase
    {
        private readonly TemperatureStampsContext _context;

        public TemperatureStampsController(TemperatureStampsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TemperatureStamp>>> GetTemperatureStamps()
        {
            return await _context.TemperatureStamps.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TemperatureStamp>> GetTemperatureStamp(long id)
        {
            var temperatureStamp = await _context.TemperatureStamps.FindAsync(id);

            if (temperatureStamp == null)
            {
                return NotFound();
            }

            return temperatureStamp;
        }

        [HttpGet("byDate")]
        public async Task<ActionResult<TemperatureStamp[]>> GetTemperatureStamp(
            [FromQuery] DateTime dateFrom, 
            [FromQuery] DateTime dateTo)
        {
            var temperatureStamp = await _context.TemperatureStamps
                .Where(item => item.Date >= dateFrom && item.Date <= dateTo)
                .ToArrayAsync();

            return temperatureStamp;
        }

        [HttpPut("{date}")]
        public async Task<IActionResult> PutTemperatureStamp(DateTime date, TemperatureStamp newTemperatureStamp)
        {
            if (date != newTemperatureStamp.Date)
            {
                return BadRequest();
            }

            var temperatureStampQuery = _context.TemperatureStamps
                .Where(item => item.Date == date);

            var temperatureStamp = await temperatureStampQuery.FirstOrDefaultAsync();
            if (temperatureStamp == null)
                return NotFound();

            _context.Entry(temperatureStamp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TemperatureStampExists(temperatureStamp.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TemperatureStamp>> PostTemperatureStamp(TemperatureStamp temperatureStamp)
        {
            var temperatureStampQuery = _context.TemperatureStamps
                .Where(item => item.Date == temperatureStamp.Date);

            if (temperatureStampQuery.Any())
                return BadRequest("Entry with that date already exists");

            _context.TemperatureStamps.Add(temperatureStamp);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostTemperatureStamp), new { id = temperatureStamp.Id }, temperatureStamp);
        }

        [HttpDelete("{date}")]
        public async Task<IActionResult> DeleteTemperatureStamp(DateTime date)
        {
            var temperatureStampQuery = _context.TemperatureStamps
                .Where(item => item.Date == date);

            var temperatureStamp = await temperatureStampQuery.FirstOrDefaultAsync();
            if (temperatureStamp == null)
                return NotFound();

            _context.TemperatureStamps.Remove(temperatureStamp);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TemperatureStampExists(long id)
        {
            return _context.TemperatureStamps.Any(e => e.Id == id);
        }
    }
}
