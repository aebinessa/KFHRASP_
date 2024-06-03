using System.ComponentModel.DataAnnotations;

namespace KFHRBackEnd.Models.Entites
{
    public class Attendance
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public DateTime CheckInTime { get; set; }
 
        public DateTime? CheckOutTime { get; set; }
    }
}
