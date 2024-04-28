namespace Thiri_Khit_GymManagementSystem.Models
{
    public class ChangePasswordRequestModel
    {
        public long UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
