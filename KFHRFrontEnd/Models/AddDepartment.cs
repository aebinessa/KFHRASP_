using System.ComponentModel.DataAnnotations;

namespace KFHRFrontEnd.Models
{
    public class AddDepartment
    {
        [Required(ErrorMessage = "Department name is required")]

        public string DepartmentName { get; set; }

        //[Required(ErrorMessage = "Department Head is required")]
        //public string Head { get; set; }

       // [Required(ErrorMessage = "Department Deatail is required")]
        //public string Detail { get; set; }

    }

}
