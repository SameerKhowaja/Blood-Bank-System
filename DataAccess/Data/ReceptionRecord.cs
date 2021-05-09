using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Data
{
    public class ReceptionRecord
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Units { get; set; }
        [Required]
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public BloodRecipient ReceivedBy { get; set; }
    }
}
