using System.ComponentModel.DataAnnotations;
namespace KFHRBackEnd.Models.Entites
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

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
       
        public Position  PositionId { get; set; }

        [Required]
        public Department DepartmentId { get; set; }

        [Required]
        public int PointEarned { get; set; }

        public bool IsAdmin { get; set; }
        private Employee() { }
        public static Employee Create(int Id, string password, bool isAdmin = false)
        {
            return new Employee
            {
                Id = Id,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password),
                IsAdmin = isAdmin
            };
        }
        public bool VerifyPassword(string pwd) => BCrypt.Net.BCrypt.EnhancedVerify(pwd, this.Password);

    }

    public enum Role
    {
        Admin,User
    }
    public enum Gender
    {
        Male, Female
    }







}
