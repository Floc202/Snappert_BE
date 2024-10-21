namespace SWD392.Snappet.API.ResponseModel
{
    public class UserResponseModel
    {
        public int UserId { get; set; }       // Unique identifier for the user
        public string Username { get; set; }  // Username of the user
        public string Email { get; set; }     // Email address of the user
        public string AccountType { get; set; } // Type of the user account (Basic, Premium, etc.)
        public DateTime CreatedAt { get; set; }  // Date when the account was created
        public DateTime UpdatedAt { get; set; }  // Last update time for the account
        public int ExpiredDays { get; set; }   // Expiration days for subscription
        public List<PetResponseModel> Pets { get; set; } = new List<PetResponseModel>(); // List of pets owned by the user
    }
}
