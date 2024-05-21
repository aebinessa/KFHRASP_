using System.ComponentModel.DataAnnotations;

namespace KFHRBackEnd.Models.Entites
{
    public class Leave
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public string LeaveType { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string Notes { get; set; }
    }
}
