namespace KFHRFrontEnd.Models.Responses
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePic { get; set; }
        public object EmployeeNfc { get; set; }
        public object EmployeeDepartmentId { get; set; }
    }
}
