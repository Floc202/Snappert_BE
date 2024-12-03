using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD392.Snappet.Repository.BusinessModels;

namespace SWD392.Snappet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PetCategoriesController : ControllerBase
    {
        private readonly SWD392_SNAPPET_DBContext _context;

        public PetCategoriesController(SWD392_SNAPPET_DBContext context)
        {
            _context = context;
        }

        // GET: api/PetCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetCategory>>> GetPetCategories()
        {
            return await _context.PetCategories.ToListAsync();
        }

        // GET: api/PetCategories/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<PetCategory>> GetPetCategory(int id)
        //{
        //    var petCategory = await _context.PetCategories.FindAsync(id);

        //    if (petCategory == null)
        //    {
        //        return NotFound();
        //    }

        //    return petCategory;
        //}

        //// PUT: api/PetCategories/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPetCategory(int id, PetCategory petCategory)
        //{
        //    if (id != petCategory.CategoryId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(petCategory).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PetCategoryExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/PetCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PetCategory>> PostPetCategory(PetCategory petCategory)
        {
            _context.PetCategories.Add(petCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPetCategory", new { id = petCategory.CategoryId }, petCategory);
        }

        // DELETE: api/PetCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePetCategory(int id)
        {
            var petCategory = await _context.PetCategories.FindAsync(id);
            if (petCategory == null)
            {
                return NotFound();
            }

            _context.PetCategories.Remove(petCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PetCategoryExists(int id)
        {
            return _context.PetCategories.Any(e => e.CategoryId == id);
        }
    }
}
