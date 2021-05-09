using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBank1.Models
{
    public class Requestor
    {
        public int request_id { get; set; }
        public string fullname { get; set; }
        public DateTime dob { get; set; }
        public string gender { get; set; }
        public string contact_number { get; set; }
        public string city { get; set; }
        public int blood_id { get; set; }
        public DateTime date_recieved { get; set; }

        public string blood_type { get; set; }
        public int unit_of_blood { get; set; }
    }
}
