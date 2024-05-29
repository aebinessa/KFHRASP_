namespace KFHRBackEnd.Models.Entites.Request.LeaveReq
{
    public class LeaveRequest
    {
        public LeaveType LeaveTypes  {get;set; }
        public DateTime StartDate { get;set; }
        public DateTime EndDate { get;set; }
    public string Notes { get;set;} 
    }

    public enum LeaveType
    {
        Anuual, Sick, emergency
    }
}
