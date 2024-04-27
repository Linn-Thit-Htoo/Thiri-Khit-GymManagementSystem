namespace Thiri_Khit_GymManagementSystem.Models
{
    public class CheckInCheckOutResponseModel
    {
        public long CICOId { get; set; }
        public string CheckInDateTime { get; set; }
        public string CheckOutDateTime { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
    }
}
