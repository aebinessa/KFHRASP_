using System.ComponentModel.DataAnnotations;

namespace KFHRBackEnd.Models.Entites
{
    public class RecommendedCertificate
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string CertificateName { get; set; }
        [Required]
        public string IssuingOrganization { get; set; }
        [Required]  
        public string OrganizationWebsite { get; set; }
        [Required]
        public string CertificatePicture { get; set; }
        [Required]
        public int RewardPoints { get; set; }
    }
}
