using System.ComponentModel.DataAnnotations;

namespace KFHRBackEnd.Models.Entites.Request.Employee
{
    public class LeaveRequest
    {
       
        public string LeaveType { get; set; } //enum
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
    }
}
