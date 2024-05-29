namespace KFHRFrontEnd.Models
{
    public class Department
    {
        public string Name { get; set; }
        public int MemberCount { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
