using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBank1.Models
{
    public class DonationDetail
    {
        public int r_id { get; set; }
        public string r_fullname { get; set; }
        public DateTime r_dob { get; set; }
        public string r_gender { get; set; }
        public string r_contact_number { get; set; }
        public string r_city { get; set; }
        public DateTime r_date_recieved { get; set; }
        public int blood_id { get; set; }
        public string blood_type { get; set; }
        public int unit_of_blood { get; set; }
        public DateTime d_date_donated { get; set; }
        public DateTime expiry_date { get; set; }
        public int d_id { get; set; }
        public string d_firstname { get; set; }
        public string d_lastname { get; set; }
        public DateTime d_dob { get; set; }
        public string d_gender { get; set; }
        public DateTime d_last_date_donation { get; set; }
        public string d_city { get; set; }
        public string d_contact_number { get; set; }
        public string d_email_id { get; set; }
        public string d_address { get; set; }
    }
}
