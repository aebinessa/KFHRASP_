using System.ComponentModel.DataAnnotations;

namespace KFHRFrontEnd.Models
{
    public class Attendancee
    {
        
        public int ID { get; set; }
        public int EmployeeId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
    }
}
