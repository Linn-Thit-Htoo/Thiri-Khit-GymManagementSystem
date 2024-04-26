using Microsoft.VisualBasic;

namespace Thiri_Khit_GymManagementSystem.Models
{
    public class MembershipResponseModel
    {
        public long MembershipId { get; set; }
        public string UserName { get; set; }
        public string PlanName { get; set; }
        public long Price { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
