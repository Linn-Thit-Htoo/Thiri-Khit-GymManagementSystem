using System.ComponentModel.DataAnnotations;

namespace Thiri_Khit_GymManagementSystem.Models
{
    public class UserDataModel
    {
        [Key]
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string UserRole { get; set; }
    }
}