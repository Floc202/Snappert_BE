namespace SWD392.Snappet.API.RequestModel
{
    public class UserRequestModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string AccountType { get; set; }
        public int? ExpiredDays { get; set; }  // Optional
        public DateTime? UpdatedAt { get; set; } // Optional, could also be handled automatically on the backend

    }
}
