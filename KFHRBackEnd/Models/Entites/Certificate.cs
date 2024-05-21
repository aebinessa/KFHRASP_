using System.ComponentModel.DataAnnotations;

namespace KFHRBackEnd.Models.Entites
{
    public class Certificate
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public string CertificateName { get; set; }
        [Required]  
        public DateTime IssueDate { get; set; }
        [Required]  
        public DateTime ExpirationDate { get; set; }
        [Required]  
        public string VerificationURL { get; set; }
    }
}
