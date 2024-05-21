using System.ComponentModel.DataAnnotations;

namespace KFHRBackEnd.Models.Entites
{
    public class LateMinutesLeft
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public TimeOnly Time { get; set; }
        [Required]
        public DateTime Month { get; set; }
    }
}
