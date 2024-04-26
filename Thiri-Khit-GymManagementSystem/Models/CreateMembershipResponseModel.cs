namespace Thiri_Khit_GymManagementSystem.Models
{
    public class CreateMembershipResponseModel
    {
        public List<UserDataModel> Users { get; set; }
        public List<MembershipPlanDataModel> Plans { get; set; }
    }
}
