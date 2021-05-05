﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloodBank_API.Models
{
    public class donorDonatedBlood_table
    {
        public int blood_id { get; set; }
        public int donor_id { get; set; }
        public string blood_type { get; set; }
        public int unit_of_blood { get; set; }
        public DateTime date_donated { get; set; }
        public DateTime expiry_date { get; set; }
        public int CheckDonate { get; set; }
    }
}