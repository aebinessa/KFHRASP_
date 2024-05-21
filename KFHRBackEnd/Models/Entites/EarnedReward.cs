using System.ComponentModel.DataAnnotations;

namespace KFHRBackEnd.Models.Entites
{
    public class EarnedReward
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int RewardId { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public DateTime RedemptionDate { get; set; }
    }
}
