using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;
using BloodBank_API.Models;

namespace BloodBank_API.Controllers
{
    public class DonationDetailController : ApiController
    {
        GenericClass gc = new GenericClass();

        // GET Doation Detail for selected columns from requestor_table join donorDonatedBlood_table join donors_table
        // Selected Columns names (r_fullname, r_date_recieved, blood_type, unit_of_blood, d_date_donated, d_firstname, d_lastname)
        [Route("getAllDonationDetail")]
        [HttpGet]
        public HttpResponseMessage getAllDonationDetail()
        {
            try
            {
                string query = "SELECT requestor_table.request_id AS r_id, requestor_table.fullname AS r_fullname, requestor_table.date_recieved AS r_date_recieved, " +
                "donorDonatedBlood_table.blood_type, donorDonatedBlood_table.unit_of_blood, donorDonatedBlood_table.date_donated AS d_date_donated, " +
                "donors_table.firstname As d_firstname, donors_table.lastname As d_lastname From requestor_table " +
                "inner join donorDonatedBlood_table on requestor_table.blood_id = donorDonatedBlood_table.blood_id " +
                "inner join donors_table on donorDonatedBlood_table.donor_id = donors_table.donor_id";

                DataTable dt = gc.GetData_Database(query);
                if (dt.Rows.Count > 0) { return Request.CreateResponse(HttpStatusCode.OK, dt); }
                else { return Request.CreateResponse(HttpStatusCode.OK, 0); }
            }
            catch
            {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.OK, -1);
            }
        }

        // GET All Doation Detail from requestor_table join donorDonatedBlood_table join donors_table using request_id
        [Route("getDonationDetailById/{id}")]
        [HttpGet]
        public HttpResponseMessage getDonationDetailById(int id)
        {
            try
            {
                string query = "SELECT requestor_table.request_id AS r_id, requestor_table.fullname AS r_fullname, requestor_table.dob AS r_dob, " +
                    "requestor_table.gender AS r_gender, requestor_table.contact_number AS r_contact_number, requestor_table.city AS r_city, " +
                    "requestor_table.date_recieved AS r_date_recieved, donorDonatedBlood_table.blood_id, donorDonatedBlood_table.blood_type, " +
                    "donorDonatedBlood_table.unit_of_blood, donorDonatedBlood_table.date_donated AS d_date_donated, donorDonatedBlood_table.expiry_date, " +
                    "donors_table.donor_id AS d_id, donors_table.firstname AS d_firstname, donors_table.lastname AS d_lastname, donors_table.dob AS d_dob, " +
                    "donors_table.gender AS d_gender, donors_table.city AS d_city, donors_table.contact_number AS d_contact_number, " +
                    "donors_table.email_id AS d_email_id, donors_table.address AS d_address From requestor_table " +
                    "inner join donorDonatedBlood_table on requestor_table.blood_id = donorDonatedBlood_table.blood_id " +
                    "inner join donors_table on donorDonatedBlood_table.donor_id = donors_table.donor_id " +
                    "WHERE requestor_table.request_id = '" + id + "'";

                DataTable dt = gc.GetData_Database(query);
                if (dt.Rows.Count > 0) { return Request.CreateResponse(HttpStatusCode.OK, dt); }
                else { return Request.CreateResponse(HttpStatusCode.OK, 0); }
            }
            catch
            {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.OK, -1);
            }
        }

    }
}
