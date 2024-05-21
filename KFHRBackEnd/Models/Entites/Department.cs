using System.ComponentModel.DataAnnotations;

namespace KFHRBackEnd.Models.Entites
{
    public class Department
    {
        [Key]
        int ID { get; set; } 
        [Required]
        public string DepartmentName { get; set; }
    }
}