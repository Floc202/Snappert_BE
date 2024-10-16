using Microsoft.AspNetCore.Mvc;
using SWD392.Snapper.Service.Services;
using SWD392.Snapper.Service.Models;
using SWD392.Snapper.Service.RequestModel;
using SWD392.Snapper.Service.ResponseModel;

namespace SWD392.Snapper.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly PetServices _petService;

        public PetsController(PetServices petService)
        {
            _petService = petService;
        }

        // GET: api/pets
        [HttpGet]
        public async Task<ActionResult<List<PetResponseModel>>> GetAllPets()
        {
            var pets = await _petService.GetAllPetsAsync();
            var petResponses = pets.Select(p => new PetResponseModel
            {
                PetId = p.PetId,
                PetName = p.PetName,
                ProfilePhotoUrl = p.ProfilePhotoUrl,
                Category = p.Category?.CategoryName, // Assuming you have a Category property on the Pet entity
                OwnerName = p.Owner?.Username, // Assuming you have an Owner property on the Pet entity
                CreatedAt = p.CreatedAt
            }).ToList();

            return Ok(petResponses);
        }

        // GET: api/pets/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PetResponseModel>> GetPetById(int id)
        {
            var pet = await _petService.GetPetByIdAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            var petResponse = new PetResponseModel
            {
                PetId = pet.PetId,
                PetName = pet.PetName,
                ProfilePhotoUrl = pet.ProfilePhotoUrl,
                Category = pet.Category?.CategoryName,
                OwnerName = pet.Owner?.Username,
                CreatedAt = pet.CreatedAt
            };

            return Ok(petResponse);
        }

        // POST: api/pets
        [HttpPost]
        public async Task<ActionResult<PetResponseModel>> CreatePet([FromBody] PetRequestModel petRequest)
        {
            var pet = new Pet
            {
                OwnerId = petRequest.OwnerId,
                PetName = petRequest.PetName,
                ProfilePhotoUrl = petRequest.ProfilePhotoUrl,
                CreatedAt = DateTime.UtcNow
            };

            var createdPetId = await _petService.CreatePetAsync(pet);
            var petResponse = new PetResponseModel
            {
                PetId = createdPetId,
                OwnerName = petRequest.OwnerId.ToString(), // Or fetch the owner's name
                PetName = pet.PetName,
                ProfilePhotoUrl = pet.ProfilePhotoUrl,
                Category = "Not Assigned", // Adjust as necessary
                CreatedAt = pet.CreatedAt
            };

            return CreatedAtAction(nameof(GetPetById), new { id = createdPetId }, petResponse);
        }

        // PUT: api/pets/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePet(int id, [FromBody] PetRequestModel petRequest)
        {
            var existingPet = await _petService.GetPetByIdAsync(id);
            if (existingPet == null)
            {
                return NotFound();
            }

            var petToUpdate = new Pet
            {
                PetId = id,
                OwnerId = petRequest.OwnerId,
                PetName = petRequest.PetName,
                ProfilePhotoUrl = petRequest.ProfilePhotoUrl,
                CreatedAt = existingPet.CreatedAt // Keep the original creation date
            };

            await _petService.UpdatePetAsync(petToUpdate);
            return NoContent();
        }

        // DELETE: api/pets/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePet(int id)
        {
            var existingPet = await _petService.GetPetByIdAsync(id);
            if (existingPet == null)
            {
                return NotFound();
            }

            await _petService.DeletePetAsync(id);
            return NoContent();
        }
    }
}
