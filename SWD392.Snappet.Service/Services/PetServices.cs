using SWD392.Snapper.Repository;
using SWD392.Snappet.Repository.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392.Snappet.Service.Services
{
    public class PetServices
    {
        private readonly UnitOfWork _unitOfWork;

        public PetServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Pet>> GetAllPetsAsync()
        {
            return await _unitOfWork.Pets.GetAllAsync();
        }

        public async Task<Pet> GetPetByIdAsync(int id)
        {
            return await _unitOfWork.Pets.GetByIdAsync(id);
        }
        public async Task<int> CreatePetAsync(Pet pet)
        {
            return await _unitOfWork.Pets.CreateAsync(pet);
        }

        public async Task<int> UpdatePetAsync(Pet pet)
        {
            return await _unitOfWork.Pets.UpdateAsync(pet);
        }
        public async Task<int> DeletePetAsync(int petId)
        {
            var pet = await _unitOfWork.Pets.GetByIdAsync(petId);
            if (pet == null) return 0;
            return await _unitOfWork.Pets.RemoveAsync(pet);
        }
    }
}
