#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MetricsManager.DAL;
using MetricsManager.Models;
using MetricsManager.Responses;
using AutoMapper;

namespace MetricsManager.Controllers
{
    [Route("api/agents")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly MetricsDbContext _context;
        private readonly IMapper _mapper;
        private const int PAGE_SIZE = 20;

        public AgentsController(MetricsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 0)
        {
            int totalEntries = await _context.AgentInfos.CountAsync();
            int totalPages = totalEntries / PAGE_SIZE;
            var response = new AgentsGetAllResponse()
            { 
                CurrentPage = page, 
                TotalPages = totalPages
            };

            foreach (var entry in _context.AgentInfos.Skip(page * PAGE_SIZE).Take(PAGE_SIZE))
            {
                response.Agents.Add(_mapper.Map<AgentInfoDto>(entry));
            }

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AgentInfo agentInfo)
        {
            _context.AgentInfos.Add(agentInfo);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }

            return CreatedAtAction(nameof(Register), new { Id = agentInfo.Id }, agentInfo);
        }

        [HttpPut("enable")]
        public async Task<IActionResult> Enable([FromQuery] int id)
        {
            return await SetEnabled(id, true);
        }

        [HttpPut("disable")]
        public async Task<IActionResult> Disable([FromQuery] int id)
        {
            return await SetEnabled(id, false);
        }

        private async Task<IActionResult> SetEnabled(int id, bool state)
        {
            var entity = await _context.AgentInfos.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            entity.Enabled = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
