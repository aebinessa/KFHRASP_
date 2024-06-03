using KFHRBackEnd.Models.Entites;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KFHRFrontEnd.Models
{
    public class EmployeeProfileDetails
    {
      
        public int Id { get; set; }

      
        public string Name { get; set; }

       
        public string Password { get; set; }

      
        public string Email { get; set; }

      
        public DateTime DOB { get; set; }

      
        public Gender Gender { get; set; }

       
        public string ProfilePicURL { get; set; }

       
        public int? NFCIdNumber { get; set; }


      
        public Position? PositionId { get; set; }

       
        public Department? DepartmentId { get; set; }


        
        public int? PointEarned { get; set; }

        public bool IsAdmin { get; set; }
    }
}
