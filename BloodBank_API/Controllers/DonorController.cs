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
        GenericClass gc = new GenericClass();

        // GET All Donors data
        [Route("getDonor")]
        [HttpGet]
        public HttpResponseMessage getDonor()
        {
            try
            {
                string query = "SELECT * from donors_table";
                DataTable dt = gc.GetData_Database(query);
                if (dt.Rows.Count > 0) { return Request.CreateResponse(HttpStatusCode.OK, dt); }
                else { return Request.CreateResponse(HttpStatusCode.BadRequest, 0); }
            }
            catch
            {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.InternalServerError, -1);
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
                DataTable dt = gc.GetData_Database(query);
                if (dt.Rows.Count > 0) { return Request.CreateResponse(HttpStatusCode.OK, dt); }
                else { return Request.CreateResponse(HttpStatusCode.BadRequest, 0); }
            }
            catch
            {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.InternalServerError, -1);
            }
        }

        // GET Donor Record Data with blood_id from donors_table
        [Route("getDonorDataByBloodID/{id}")]
        [HttpGet]
        public HttpResponseMessage getDonorDataByBloodID(int id)
        {
            try
            {
                string query = "SELECT * from donors_table WHERE donor_id = (SELECT donor_id from donorDonatedBlood_table WHERE blood_id = '" + id + "');";
                DataTable dt = gc.GetData_Database(query);
                if (dt.Rows.Count > 0) { return Request.CreateResponse(HttpStatusCode.OK, dt); }
                else { return Request.CreateResponse(HttpStatusCode.BadRequest, 0); }
            }
            catch
            {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.InternalServerError, -1);
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
                DataTable dt = gc.GetData_Database(query);
                if (dt.Rows.Count > 0) { return Request.CreateResponse(HttpStatusCode.OK, dt); }
                else { return Request.CreateResponse(HttpStatusCode.BadRequest, 0); }
            }
            catch
            {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.InternalServerError, -1);
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
                int i = gc.PostUpdateDeleteData_Database(query);

                // success
                if (i == 1){ return Request.CreateResponse(HttpStatusCode.OK, 1); } // 200
                // failed
                else{ return Request.CreateResponse(HttpStatusCode.BadRequest, 0); } //400
            }
            catch{ 
                // Error occured
                return Request.CreateResponse(HttpStatusCode.InternalServerError, -1); // 500
            }
        }

        // POST Add new Blood Donation of Donor to donorDonatedBlood_table via donor_id
        [Route("addNewDonorDonation")]
        [HttpPost]
        public HttpResponseMessage addNewDonorDonation(donorDonatedBlood_table bloodDonationTB)
        {
            try
            {
                string query = "SELECT blood_type FROM donors_table WHERE donor_id = '" + bloodDonationTB.donor_id + "'";
                DataTable dt = gc.GetData_Database(query);
                if (dt.Rows.Count > 0) {
                    string bloodType = dt.Rows[0].Field<string>(0); // bloodType
                    bloodDonationTB.blood_type = bloodType;

                    string query1 = "INSERT INTO donorDonatedBlood_table (donor_id, blood_type, unit_of_blood, date_donated, expiry_date) VALUES ('" + bloodDonationTB.donor_id + "'," +
                    " '" + bloodDonationTB.blood_type + "', '" + bloodDonationTB.unit_of_blood + "', '" + bloodDonationTB.date_donated + "', '" + bloodDonationTB.expiry_date + "')";
                    int i = gc.PostUpdateDeleteData_Database(query1);

                    if (i == 1)
                    {
                        // On success update last donation date of donor in donors_table
                        string query2 = "UPDATE donors_table SET last_date_donation = '" + bloodDonationTB.date_donated + "' WHERE donor_id = '" + bloodDonationTB.donor_id + "'";
                        int j = gc.PostUpdateDeleteData_Database(query2);

                        if (j == 1)
                        {
                            // On success add data to bloodbankStock_table if blood not expired
                            if(DateTime.Now <= bloodDonationTB.expiry_date)
                            {
                                string query3 = "UPDATE bloodbankStock_table SET total_units = total_units + '" + bloodDonationTB.unit_of_blood + "' WHERE blood_type = '" + bloodDonationTB.blood_type + "'";
                                int k = gc.PostUpdateDeleteData_Database(query3);

                                // success
                                if (k == 1) { return Request.CreateResponse(HttpStatusCode.OK, 1); }
                                // failed
                                else { return Request.CreateResponse(HttpStatusCode.BadRequest, 0); }
                            }
                            else
                            {
                                // No Action just return OK 1
                                return Request.CreateResponse(HttpStatusCode.OK, 1);
                            }
                        }
                        else
                        {
                            //failed
                            return Request.CreateResponse(HttpStatusCode.BadRequest, 0);
                        }
                    }
                    else
                    {
                        // failed
                        return Request.CreateResponse(HttpStatusCode.BadRequest, 0);
                    }
                }
                else
                {
                    // failed
                    return Request.CreateResponse(HttpStatusCode.BadRequest, 0);
                }
            }
            catch
            {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.InternalServerError, -1);
            }
        }

        // Update Donor record via donor_id in donors_table
        [Route("editDonor")]
        [HttpPut]
        public HttpResponseMessage editDonor(donors_table donorTB)
        {
            try
            {
                string query = "UPDATE donors_table SET firstname = '" + donorTB.firstname + "', lastname = '" + donorTB.lastname + "', dob = '" + donorTB.dob + "', gender = '" + donorTB.gender + "', blood_type = '" + donorTB.blood_type + "', " +
                    "last_date_donation = '" + donorTB.last_date_donation + "', contact_number = '" + donorTB.contact_number + "', email_id = '" + donorTB.email_id + "', city = '" + donorTB.city + "', address = '" + donorTB.address + "' " +
                    "WHERE donor_id = '" + donorTB.donor_id + "'";

                int i = gc.PostUpdateDeleteData_Database(query);
                // On success update blood_type on donorDonatedBlood_table with donor_id
                if (i == 1) {
                    // Check if blood record present in donorDonatedBlood_table of this donor
                    string query1 = "SELECT * FROM donorDonatedBlood_table WHERE donor_id = '" + donorTB.donor_id + "'";
                    DataTable dt = gc.GetData_Database(query1);
                    if (dt.Rows.Count > 0)
                    {
                        // Means donor have some blood donations
                        // Update blood_type on donorDonatedBlood_table with donor_id

                        string query2 = "UPDATE donorDonatedBlood_table SET blood_type = '" + donorTB.blood_type + "' WHERE donor_id = '" + donorTB.donor_id + "'";
                        int c = gc.PostUpdateDeleteData_Database(query2);
                        // success (multiple rows effected)
                        if (c >= 1) { return Request.CreateResponse(HttpStatusCode.OK, 1); }
                        // failed
                        else { return Request.CreateResponse(HttpStatusCode.BadRequest, 0); }
                    }
                    else
                    {
                        // Means donor have no donation of blood
                        return Request.CreateResponse(HttpStatusCode.OK, 1);
                    }
                }
                // failed
                else { 
                    return Request.CreateResponse(HttpStatusCode.BadRequest, 0); 
                }
            }
            catch
            {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.InternalServerError, -1);
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
                int i = gc.PostUpdateDeleteData_Database(query);

                // success
                if (i == 1) { return Request.CreateResponse(HttpStatusCode.OK, 1); }
                // failed
                else { return Request.CreateResponse(HttpStatusCode.BadRequest, 0); }
            }
            catch
            {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.InternalServerError, -1);
            }
        }


    }
}
