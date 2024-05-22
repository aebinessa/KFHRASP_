using System.ComponentModel.DataAnnotations;

namespace KFHRBackEnd.Models.Entites
{
    public class AddEmployees
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public Role Role { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [Url]
        public string ProfilePicURL { get; set; }

        [Required]
        public NFC NFCId { get; set; }

        [Required]

        public Position PositionId { get; set; }

        [Required]
        public Department DepartmentId { get; set; }

        [Required]
        public int PointEarned { get; set; }
    }
}
