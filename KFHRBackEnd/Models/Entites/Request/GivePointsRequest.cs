using System.ComponentModel.DataAnnotations;

namespace KFHRBackEnd.Models.Entites.Request
{
    public class GivePointsRequest
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Points must be a positive number.")]
        public int Points { get; set; }
    }
}
