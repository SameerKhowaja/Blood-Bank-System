using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class BloodDonorDTO
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string BloodType { get; set; }
        [Required]
        public DateTime LastDonatedAt { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
    }
}
