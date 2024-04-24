using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thiri_Khit_GymManagementSystem.Models
{
    [Table("Membership_Plan")]
    public class MembershipPlanDataModel
    {
        [Key]
        public long MembershipPlanId { get; set; }
        public string PlanName { get; set; }
        public long Price { get; set; }
    }
}
