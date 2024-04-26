using System.ComponentModel.DataAnnotations;

namespace Thiri_Khit_GymManagementSystem.Models
{
    public class MembershipDataModel
    {
        [Key]
        public long MembershipId { get; set; }
        public long MembershipPlanId { get; set; }
        public long UserId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
