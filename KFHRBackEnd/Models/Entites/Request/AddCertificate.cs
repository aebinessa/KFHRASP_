using System.ComponentModel.DataAnnotations;

namespace KFHRBackEnd.Models.Entites.Request
{
    public class AddCertificate
    {
        [Required]
        public string CertificateName { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        [Url]
        public string VerificationURL { get; set; }
    }
}
