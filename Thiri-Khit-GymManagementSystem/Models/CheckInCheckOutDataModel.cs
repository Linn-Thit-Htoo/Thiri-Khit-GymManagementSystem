using System.ComponentModel.DataAnnotations;

namespace Thiri_Khit_GymManagementSystem.Models
{
    public class CheckInCheckOutDataModel
    {
        [Key]
        public long CICOId { get; set; }
        public long UserId { get; set; }
        public string CheckInDateTime { get; set; }
        public string? CheckOutDateTime { get; set;}

        public static implicit operator bool(CheckInCheckOutDataModel? v)
        {
            throw new NotImplementedException();
        }
    }
}
