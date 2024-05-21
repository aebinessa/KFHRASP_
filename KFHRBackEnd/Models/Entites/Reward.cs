using System.ComponentModel.DataAnnotations;

namespace KFHRBackEnd.Models.Entites
{
    public class Reward
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string RewardName { get; set; }
        [Required]
        public int PointsRequired { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
