using System.ComponentModel.DataAnnotations;

namespace KFHRBackEnd.Models
{
    public class UpdateLeaveStatus
    {
        [Required]
        public string Status { get; set; }
    }
}
