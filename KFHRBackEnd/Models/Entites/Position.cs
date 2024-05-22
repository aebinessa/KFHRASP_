using System.ComponentModel.DataAnnotations;

namespace KFHRBackEnd.Models.Entites
{
    public class Position
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string TitleString { get; set; }
    }
}