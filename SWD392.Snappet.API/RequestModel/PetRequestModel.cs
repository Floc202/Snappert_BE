namespace SWD392.Snappet.API.RequestModel
{
    public class PetRequestModel
    {
        public string PetName { get; set; }    // Name of the pet
        public int OwnerId { get; set; }       // ID of the user who owns the pet
        public string ProfilePhotoUrl { get; set; }  // URL for the pet's profile photo
        public int CategoryId { get; set; }    // Category or type of the pet (e.g., Dog, Cat)
        public string Description { get; set; } // Description for pet
    }
}
