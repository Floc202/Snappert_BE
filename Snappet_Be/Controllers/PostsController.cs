using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Snappet_Be.Data;
using Snappet_Be.Models;

namespace Snappet_Be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly SWD392_SNAPPET_DBContext _context;

        public PostsController(SWD392_SNAPPET_DBContext context)
        {
            _context = context;
        }

        // GET: api/posts
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts
                .Include(p => p.Photo)
                .Include(p => p.User)
                .ToListAsync();
            return Ok(posts); // Return the list of posts
        }

        // GET: api/posts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Photo)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post); // Return the post details
        }

        // POST: api/posts
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Post post)
        {
            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Details), new { id = post.PostId }, post); // Return 201 Created
            }
            return BadRequest(ModelState); // Return 400 Bad Request
        }

        // PUT: api/posts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Post post)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
                    {
                        return NotFound();
                    }
                    throw; // Re-throw the exception if the post exists
                }
                return NoContent(); // Return 204 No Content
            }
            return BadRequest(ModelState); // Return 400 Bad Request
        }

        // DELETE: api/posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return NoContent(); // Return 204 No Content
            }

            return NotFound(); // Return 404 Not Found if the post does not exist
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
