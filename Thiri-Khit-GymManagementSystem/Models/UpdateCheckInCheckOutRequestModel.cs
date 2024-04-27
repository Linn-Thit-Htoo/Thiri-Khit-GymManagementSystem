namespace Thiri_Khit_GymManagementSystem.Models
{
    public class UpdateCheckInCheckOutRequestModel
    {
        public long CICOId { get; set; }
        public long UserId { get; set; }
        public string CheckInDateTime { get; set; }
    }
}
