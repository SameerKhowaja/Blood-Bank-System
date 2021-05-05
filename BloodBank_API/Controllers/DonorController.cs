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
    public class DonorController : ApiController
    {
        static string conStr = ConfigurationManager.ConnectionStrings["connectDB"].ConnectionString;
        SqlConnection con = new SqlConnection(conStr);
        GenericClass gc = new GenericClass();

        // GET All Donors data
        [Route("getDonor")]
        [HttpGet]
        public HttpResponseMessage getDonor()
        {
            try
            {
                string query = "SELECT * from donors_table";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gc.TrimDataTableRow(dt);

                if (dt.Rows.Count > 0)
                {
                    // success
                    return Request.CreateResponse(HttpStatusCode.OK, dt);
                }
                else
                {
                    // failed 0 Rows returned
                    return Request.CreateResponse(HttpStatusCode.OK, 0);
                }
            }
            catch
            {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.OK, -1);
            }
        }

        // GET Donor Record with donor_id from donors_table
        [Route("getDonor/{id}")]
        [HttpGet]
        public HttpResponseMessage getDonorByID(int id)
        {
            try
            {
                string query = "SELECT * from donors_table WHERE donor_id = '" + id + "'";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gc.TrimDataTableRow(dt);

                if (dt.Rows.Count > 0)
                {
                    // success
                    return Request.CreateResponse(HttpStatusCode.OK, dt);
                }
                else
                {
                    // failed 0 Rows returned
                    return Request.CreateResponse(HttpStatusCode.OK, 0);
                }
            }
            catch
            {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.OK, -1);
            }
        }

        // GET Donor blood donation record with donor_id from donorDonatedBlood_table
        [Route("getDonorDonation/{id}")]
        [HttpGet]
        public HttpResponseMessage getDonorDonation(int id)
        {
            try
            {
                string query = "SELECT * from donorDonatedBlood_table WHERE donor_id = '" + id + "'";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gc.TrimDataTableRow(dt);

                if (dt.Rows.Count > 0)
                {
                    // success
                    return Request.CreateResponse(HttpStatusCode.OK, dt);
                }
                else
                {
                    // failed 0 Rows returned
                    return Request.CreateResponse(HttpStatusCode.OK, 0);
                }
            }
            catch
            {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.OK, -1);
            }
        }

        // POST Add new Donor to donors_table
        [Route("addNewDonor")]
        [HttpPost]
        public HttpResponseMessage addNewDonor(donors_table donorTB)
        {
            try
            {
                string query = "INSERT INTO donors_table (firstname, lastname, dob, gender, blood_type, last_date_donation, contact_number, email_id, city, address) VALUES ('" + donorTB.firstname + "', '" + donorTB.lastname + "', " +
                    "'" + donorTB.dob + "', '" + donorTB.gender + "', '" + donorTB.blood_type + "', '" + donorTB.last_date_donation + "', '" + donorTB.contact_number + "', '" + donorTB.email_id + "', '" + donorTB.city + "', '" + donorTB.address + "')";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i == 1)
                {
                    // success
                    return Request.CreateResponse(HttpStatusCode.OK, 1);
                }
                else
                {
                    // failed
                    return Request.CreateResponse(HttpStatusCode.OK, 0);
                }
            }
            catch{ 
                // Error occured
                return Request.CreateResponse(HttpStatusCode.OK, -1); 
            }
        }

        // POST Add new Blood Donation of Donor to donorDonatedBlood_table via donor_id
        [Route("addNewDonorDonation")]
        [HttpPost]
        public HttpResponseMessage addNewDonorDonation(donorDonatedBlood_table bloodDonationTB)
        {
            try
            {
                string query = "INSERT INTO donorDonatedBlood_table (donor_id, blood_type, unit_of_blood, date_donated, expiry_date) VALUES ('" + bloodDonationTB.donor_id + "'," +
                    " '" + bloodDonationTB.blood_type + "', '" + bloodDonationTB.unit_of_blood + "', '" + bloodDonationTB.date_donated + "', '" + bloodDonationTB.expiry_date + "')";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i == 1)
                {
                    // on success add data to bloodbankStock_table
                    string query2 = "UPDATE bloodbankStock_table SET total_units = total_units + '" + bloodDonationTB.unit_of_blood + "' WHERE blood_type = '" + bloodDonationTB.blood_type + "'";
                    SqlCommand cmd2 = new SqlCommand(query2, con);
                    cmd2.CommandType = CommandType.Text;
                    con.Open();
                    int j = cmd2.ExecuteNonQuery();
                    con.Close();

                    // success
                    if (j == 1)
                        return Request.CreateResponse(HttpStatusCode.OK, 1);
                    // failed
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, 0);
                }
                else
                {
                    // failed
                    return Request.CreateResponse(HttpStatusCode.OK, 0);
                }
            }
            catch
            {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.OK, -1);
            }
        }

        // PUT Update Donor record via donor_id in donors_table
        [Route("editDonor/{id}")]
        [HttpPut]
        public HttpResponseMessage editDonor(donors_table donorTB)
        {
            try
            {
                string query = "UPDATE donors_table SET firstname = '" + donorTB.firstname + "', lastname = '" + donorTB.lastname + "', dob = '" + donorTB.dob + "', " +
                    "contact_number = '" + donorTB.contact_number + "', email_id = '" + donorTB.email_id + "', city = '" + donorTB.city + "', address = '" + donorTB.address + "' " +
                    "WHERE donor_id = '" + donorTB.donor_id + "'";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i == 1)
                {
                    // success
                    return Request.CreateResponse(HttpStatusCode.OK, 1);
                }
                else
                {
                    // failed
                    return Request.CreateResponse(HttpStatusCode.OK, 0);
                }
            }
            catch
            {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.OK, -1);
            }
        }

        // DELETE Donor with donor_id from donors_table
        [Route("deleteDonor/{id}")]
        [HttpDelete]
        public HttpResponseMessage deleteDonor(int id)
        {
            try
            {
                string query = "DELETE From donors_table WHERE donor_id = '" + id + "'";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i == 1)
                {
                    // success
                    return Request.CreateResponse(HttpStatusCode.OK, 1);
                }
                else
                {
                    // failed
                    return Request.CreateResponse(HttpStatusCode.OK, 0);
                }
            }
            catch
            {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.OK, -1);
            }
        }


    }
}
