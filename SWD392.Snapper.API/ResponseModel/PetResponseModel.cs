namespace Snappet_Be.ResponseModel
{
    public class PetResponseModel
    {
        public int PetId { get; set; }        // Unique ID for the pet
        public string PetName { get; set; }   // Name of the pet
        public string ProfilePhotoUrl { get; set; }  // URL for the pet's profile photo
        public string Category { get; set; }  // Category or type of the pet
        public string OwnerName { get; set; }  // Name of the pet's owner (the user)
        public DateTime CreatedAt { get; set; }  // Date when the pet was added to the system
    }
}
