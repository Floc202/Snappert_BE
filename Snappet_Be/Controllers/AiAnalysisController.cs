using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Snappet_Be.Data;
using Snappet_Be.Models;

namespace Snappet_Be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiAnalysisController : ControllerBase
    {
        private readonly SWD392_SNAPPET_DBContext _context;

        public AiAnalysisController(SWD392_SNAPPET_DBContext context)
        {
            _context = context;
        }

        // GET: api/AiAnalysis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AiAnalysis>>> GetAiAnalyses()
        {
            return await _context.AiAnalyses.ToListAsync();
        }

        // GET: api/AiAnalysis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AiAnalysis>> GetAiAnalysis(int id)
        {
            var aiAnalysis = await _context.AiAnalyses.FindAsync(id);

            if (aiAnalysis == null)
            {
                return NotFound();
            }

            return aiAnalysis;
        }

        // PUT: api/AiAnalysis/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAiAnalysis(int id, AiAnalysis aiAnalysis)
        {
            if (id != aiAnalysis.AnalysisId)
            {
                return BadRequest();
            }

            _context.Entry(aiAnalysis).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AiAnalysisExists(id))
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

        // POST: api/AiAnalysis
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AiAnalysis>> PostAiAnalysis(AiAnalysis aiAnalysis)
        {
            _context.AiAnalyses.Add(aiAnalysis);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAiAnalysis", new { id = aiAnalysis.AnalysisId }, aiAnalysis);
        }

        // DELETE: api/AiAnalysis/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAiAnalysis(int id)
        {
            var aiAnalysis = await _context.AiAnalyses.FindAsync(id);
            if (aiAnalysis == null)
            {
                return NotFound();
            }

            _context.AiAnalyses.Remove(aiAnalysis);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AiAnalysisExists(int id)
        {
            return _context.AiAnalyses.Any(e => e.AnalysisId == id);
        }
    }
}
