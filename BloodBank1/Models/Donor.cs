using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBank1.Models
{
    public class Donor
    {
        public int donor_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public DateTime dob { get; set; }
        public string gender { get; set; }
        public string blood_type { get; set; }
        public DateTime last_date_donation { get; set; }
        public string contact_number { get; set; }
        public string email_id { get; set; }
        public string city { get; set; }
        public string address { get; set; }
    }
}
