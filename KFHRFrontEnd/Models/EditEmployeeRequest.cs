namespace KFHRFrontEnd.Models
{
    public class EditEmployeeRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string ProfilePicURL { get; set; }
        public string NFCIdNumber { get; set; }
        public int PositionId { get; set; }
        public int DepartmentId { get; set; }
        public int PointEarned { get; set; }
    }
}
