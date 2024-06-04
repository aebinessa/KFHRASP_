using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KFHRBackEnd.Models.Entites
{
    public class LateMinutesLeft
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        [AllowNull]
        public TimeOnly? Time { get; set; }

        [Required]
        public int MinutesLeft { get; set; }

        [Required]
        public DateTime Month { get; set; }
       
    }
}
