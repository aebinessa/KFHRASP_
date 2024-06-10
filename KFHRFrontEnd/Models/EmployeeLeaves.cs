namespace KFHRFrontEnd.Models
{
    public class EmployeeLeaves
    {
        public int Id { get; set; }
        public int employeeId { get; set; }
        public string employeeName { get; set; }
        public string leaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string notes { get; set; }
        public string Status { get; set; }

    }
}
