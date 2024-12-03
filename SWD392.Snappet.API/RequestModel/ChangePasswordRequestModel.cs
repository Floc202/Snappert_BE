namespace SWD392.Snappet.API.RequestModel
{
    public class ChangePasswordRequestModel
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
