using SWD392.Snappet.Repository.BusinessModels;

namespace SWD392.Snappet.API.ResponseModel
{
    public class PetResponseModel
    {
        public int PetId { get; set; }        // Unique ID for the pet
        public string PetName { get; set; }   // Name of the pet
        public string ProfilePhotoUrl { get; set; }  // URL for the pet's profile photo
        public string PetCategoryName { get; set; }  // Category or type of the pet
        public string OwnerName { get; set; }  // Name of the pet's owner (the user)
        public DateTime CreatedAt { get; set; }  // Date when the pet was added to the system
        public string Description { get; set; }
        public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
    }
}
