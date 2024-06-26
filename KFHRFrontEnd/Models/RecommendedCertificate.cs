﻿using System.ComponentModel.DataAnnotations;

namespace KFHRFrontEnd.Models
{
    public class RecommendedCertificate
    {
        public string CertificateName { get; set; }
        
        public string IssuingOrganization { get; set; }
        
        public string OrganizationWebsite { get; set; }
        
        public string CertificatePicture { get; set; }
        
        public int RewardPoints { get; set; }

    }
}
