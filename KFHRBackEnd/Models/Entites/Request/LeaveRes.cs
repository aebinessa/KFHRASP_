namespace KFHRBackEnd.Models.Entites.Request
{
    public class LeaveRes
    {
        public int ID { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
    }
}
