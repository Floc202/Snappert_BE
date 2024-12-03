using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD392.Snappet.API.RequestModel;
using SWD392.Snappet.API.ResponseModel;
using SWD392.Snappet.Repository.BusinessModels;

namespace SWD392.Snappet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PetsController : ControllerBase
    {
        private readonly SWD392_SNAPPET_DBContext _context;

        public PetsController(SWD392_SNAPPET_DBContext context)
        {
            _context = context;
        }
        //[HttpGet]
        //public IActionResult GetSecureData()
        //{
        //    return Ok("This is protected data.");
        //}
        // GET: api/Pets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pet>>> GetPets()
        {
            var pets = await _context.Pets.ToListAsync();
            var petResponses = new List<PetResponseModel>();

            foreach (var pet in pets)
            {
                var petResponse = await MapToPetResponseModelAsync(pet);
                petResponses.Add(petResponse);
            }

            return Ok(petResponses);

        }

        private async Task<PetResponseModel> MapToPetResponseModelAsync(Pet pet)
        {
            //var category = await _context.PetCategories.FindAsync(pet.CategoryId);
            //var owner = await _context.Users.FindAsync(pet.UserId);

            var photos = await _context.Photos.Where(p => p.PetId == pet.PetId).ToListAsync();

            //var activePhotos = photos.Where(photo => (bool)photo.Status).ToList();

            return new PetResponseModel
            {
                PetId = pet.PetId,
                PetName = pet.PetName,
                ProfilePhotoUrl = pet.ProfilePhotoUrl,
                PetCategoryName = pet.Category?.CategoryName ?? "Unknow",
                OwnerName = pet.User?.Username ?? "Unknow",
                CreatedAt = pet.CreatedAt,
                Description = pet.Description,
                Photos = pet.Photos.Select(photo => new Photo
                {
                    PhotoId = photo.PhotoId, // Non-nullable
                    PhotoUrl = photo.PhotoUrl ?? "default-url.jpg", // Default for null PhotoUrl
                    Tags = photo.Tags ?? string.Empty, // Default to empty string if null
                    CreatedAt = photo.CreatedAt, // Non-nullable
                    Status = photo.Status ?? false // Default to false if Status is null
                }).ToList(),
            };
        }

        // GET: api/Pets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PetResponseModel>> GetPet(int id)
        {
            var pet = await _context.Pets.FindAsync(id);

            if (pet == null)
            {
                return NotFound();
            }

            var petResponse = await MapToPetResponseModelAsync(pet);
            return Ok(petResponse);
        }

      
        // PATCH: api/Pets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PatchPet(int id, PetRequestModel petRequest)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            var categoryExists = await _context.PetCategories.AnyAsync(c => c.CategoryId == petRequest.CategoryId);
            var ownerExists = await _context.Users.AnyAsync(u => u.UserId == petRequest.OwnerId);

            if (!categoryExists)
            {
                return BadRequest("Invalid CategoryId.");
            }

            if (!ownerExists)
            {
                return BadRequest("Invalid OwnerId.");
            }

            pet.PetName = petRequest.PetName;
            pet.ProfilePhotoUrl = petRequest.ProfilePhotoUrl;
            pet.CategoryId = petRequest.CategoryId;
            pet.Description = petRequest.Description;
            pet.UserId = petRequest.OwnerId;

            _context.Entry(pet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var petResponse = await MapToPetResponseModelAsync(pet);
            return Ok(petResponse);
        }

        // POST: api/Pets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pet>> PostPet(PetRequestModel petRequest)
        {
            var categoryExists = await _context.PetCategories.AnyAsync(c => c.CategoryId == petRequest.CategoryId);
            var ownerExists = await _context.Users.AnyAsync(u => u.UserId == petRequest.OwnerId);

            if (!categoryExists)
            {
                return BadRequest("Invalid CategoryId.");
            }

            if (!ownerExists)
            {
                return BadRequest("Invalid OwnerId.");
            }

            var nextPetId = _context.Pets.Any() ? _context.Pets.Max(p => p.PetId) + 1 : 1;

            var pet = new Pet
            {
                PetId = nextPetId,
                PetName = petRequest.PetName,
                ProfilePhotoUrl = petRequest.ProfilePhotoUrl,
                CategoryId = petRequest.CategoryId,
                Description = petRequest.Description,
                UserId = petRequest.OwnerId,
                CreatedAt = DateTime.UtcNow // Use UtcNow to avoid SqlDateTime overflow issues
            };

            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPet", new { id = pet.PetId }, pet);
        }

        // DELETE: api/Pets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePet(int id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            var photos = await _context.Photos.Where(p => p.PetId == id).ToListAsync();
            foreach (var photo in photos)
            {
                photo.Status = false;
                photo.PetId = pet.PetId;
                _context.Entry(photo).State = EntityState.Modified;
            }

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Delete pet successfully." });
        }

        private bool PetExists(int id)
        {
            return _context.Pets.Any(e => e.PetId == id);
        }

    }
}
