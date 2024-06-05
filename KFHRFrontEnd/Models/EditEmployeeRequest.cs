namespace KFHRFrontEnd.Models
{
    public class EditEmployeeRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? DOB { get; set; }
        public Gender Gender { get; set; }
        public string ProfilePicURL { get; set; }
        public string? PositionName { get; set; }
        public int? DepartmentId { get; set; }
        public int PointEarned { get; set; }
    }

}
