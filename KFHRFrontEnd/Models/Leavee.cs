using System.ComponentModel.DataAnnotations;

namespace KFHRFrontEnd.Models
{
    public class Leavee
    {
        
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
