namespace Thiri_Khit_GymManagementSystem.Models
{
    public class EditMembershipResponseModel
    {
        public MembershipDataModel Membership { get; set; }
        public List<UserDataModel> Users { get; set; }
        public List<MembershipPlanDataModel> Plans { get; set; }
    }
}
