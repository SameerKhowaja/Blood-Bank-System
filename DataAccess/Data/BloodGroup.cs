using System.ComponentModel.DataAnnotations;

namespace DataAccess.Data
{
    public class BloodGroup
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string BloodType { get; set; }
        [Required]
        public int Units { get; set; }
    }
}
