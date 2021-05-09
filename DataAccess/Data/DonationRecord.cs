using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Data
{
    public class DonationRecord
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Units { get; set; }
        [Required]
        public DateTime DonatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public BloodDonor DonatedBy { get; set; }
    }
}
