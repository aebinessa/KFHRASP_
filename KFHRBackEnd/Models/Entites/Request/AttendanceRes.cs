namespace KFHRBackEnd.Models.Entites.Request
{
    public class AttendanceRes
    {
        public int ID { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }
}
