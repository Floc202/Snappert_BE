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
    public class AdminActionsController : ControllerBase
    {
        private readonly SWD392_SNAPPET_DBContext _context;

        public AdminActionsController(SWD392_SNAPPET_DBContext context)
        {
            _context = context;
        }

        // GET: api/AdminActions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminAction>>> GetAdminActions()
        {
            return await _context.AdminActions.ToListAsync();
        }

        // GET: api/AdminActions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminAction>> GetAdminAction(int id)
        {
            var adminAction = await _context.AdminActions.FindAsync(id);

            if (adminAction == null)
            {
                return NotFound();
            }

            return adminAction;
        }

        // PUT: api/AdminActions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdminAction(int id, AdminAction adminAction)
        {
            if (id != adminAction.ActionId)
            {
                return BadRequest();
            }

            _context.Entry(adminAction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminActionExists(id))
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

        // POST: api/AdminActions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdminAction>> PostAdminAction(AdminAction adminAction)
        {
            _context.AdminActions.Add(adminAction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdminAction", new { id = adminAction.ActionId }, adminAction);
        }

        // DELETE: api/AdminActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdminAction(int id)
        {
            var adminAction = await _context.AdminActions.FindAsync(id);
            if (adminAction == null)
            {
                return NotFound();
            }

            _context.AdminActions.Remove(adminAction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdminActionExists(int id)
        {
            return _context.AdminActions.Any(e => e.ActionId == id);
        }
    }
}
