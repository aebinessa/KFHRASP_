namespace KFHRFrontEnd.Models
{
    public class EmployeeCertificate
    {
        public string CertificateName { get; set; }

        public DateTime issueDate { get; set; }

        public DateTime expirationDate { get; set; }

        public string verificationURL { get; set; }

        
    }
}
