using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloodBank_API.Models
{
    public class requestor_table
    {
        public int request_id { get; set; }
        public string fullname { get; set; }
        public DateTime dob { get; set; }
        public string gender { get; set; }
        public string contact_number { get; set; }
        public string city { get; set; }
        public int blood_id { get; set; }
        public DateTime date_recieved { get; set; }
    }
}