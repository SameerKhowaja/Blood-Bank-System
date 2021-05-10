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
    public class BloodDetailController : ApiController
    {
        GenericClass gc = new GenericClass();

        // GET All Blood Detail (Available or Expired or Consumed) from donorDonatedBlood_table with donors_table information
        [Route("getAllBloodWithDonor")]
        [HttpGet]
        public HttpResponseMessage getAllBloodWithDonor()
        {
            try
            {
                string query = "SELECT donorDonatedBlood_table.*, donors_table.* " +
                "FROM donorDonatedBlood_table inner join donors_table " +
                "ON donorDonatedBlood_table.donor_id = donors_table.donor_id";

                DataTable dt = gc.GetData_Database(query);
                dt.Columns.Remove("donor_id1");
                if (dt.Rows.Count > 0) { return Request.CreateResponse(HttpStatusCode.OK, dt); }
                else { return Request.CreateResponse(HttpStatusCode.OK, 0); }
            }
            catch
            {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.OK, -1);
            }
        }

        // GET Donor ID from blood_id from donorDonatedBlood_table
        [Route("getDonorIdByBloodId/{id}")]
        [HttpGet]
        public HttpResponseMessage getDonorDataByBloodID(int id)
        {
            try
            {
                string query = "SELECT donor_id from donorDonatedBlood_table WHERE blood_id = '" + id + "' AND CheckDonate = 0";

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

        // GET All Blood Details (Available or Expired or Consumed) from donorDonatedBlood_table
        [Route("getAllBloodDetail")]
        [HttpGet]
        public HttpResponseMessage getAllBloodDetail()
        {
            try
            {
                string query = "SELECT * FROM donorDonatedBlood_table";
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

        // GET All Blood Details (blood_type, unit_of_blood, expiry_date, CheckDonate) from donorDonatedBlood_table
        [Route("getAllBloodData")]
        [HttpGet]
        public HttpResponseMessage getAllBloodData()
        {
            try
            {
                string query = "SELECT blood_type, unit_of_blood, expiry_date, CheckDonate FROM donorDonatedBlood_table";
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

        // GET All Available Blood Details (NOT Expired or Consumed) from donorDonatedBlood_table
        [Route("getAllAvailableBloodDetail")]
        [HttpGet]
        public HttpResponseMessage getAllAvailableBloodDetail()
        {
            try
            {
                string query = "SELECT * FROM donorDonatedBlood_table WHERE CheckDonate = 0 AND expiry_date >= CURRENT_TIMESTAMP";
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

        // GET All Available Blood (blood_type, quantity) from donorDonatedBlood_table
        [Route("getBlood/available")]
        [HttpGet]
        public HttpResponseMessage getBloodAvailable()
        {
            try
            {
                string query = "SELECT blood_type, SUM(unit_of_blood) AS unit_of_blood FROM donorDonatedBlood_table " +
                "WHERE CheckDonate = 0 AND expiry_date >= CURRENT_TIMESTAMP GROUP BY blood_type";

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

        // GET All Consumed Blood (blood_type, quantity) from donorDonatedBlood_table
        [Route("getBlood/consumed")]
        [HttpGet]
        public HttpResponseMessage getBloodConsumed()
        {
            try
            {
                string query = "SELECT blood_type, SUM(unit_of_blood) AS unit_of_blood FROM donorDonatedBlood_table " +
                "WHERE CheckDonate = 1 GROUP BY blood_type";

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

        // GET All Expired Blood (blood_type, quantity) from donorDonatedBlood_table
        [Route("getBlood/expired")]
        [HttpGet]
        public HttpResponseMessage getBloodExpired()
        {
            try
            {
                string query = "SELECT blood_type, SUM(unit_of_blood) AS unit_of_blood FROM donorDonatedBlood_table " +
                "WHERE expiry_date <= CURRENT_TIMESTAMP GROUP BY blood_type";

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

        // Delete Blood Data by blood_id from donorDonatedBlood_table
        [Route("deleteBloodData/{id}")]
        [HttpDelete]
        public HttpResponseMessage deleteBloodData(int id)
        {
            try
            {
                string query = "SELECT blood_type, unit_of_blood FROM donorDonatedBlood_table WHERE blood_id = '" + id + "'";
                DataTable dt = gc.GetData_Database(query);

                if (dt.Rows.Count > 0) {
                    string bloodType = dt.Rows[0].Field<string>(0); // bloodType
                    int unit_of_blood = dt.Rows[0].Field<int>(1);   // unitOfBlood

                    string query1 = "DELETE From donorDonatedBlood_table WHERE blood_id = '" + id + "'";
                    int i = gc.PostUpdateDeleteData_Database(query1);
                    // success
                    if (i == 1)
                    {
                        string query2 = "UPDATE bloodbankStock_table SET total_units = total_units - '" + unit_of_blood + "' WHERE blood_type = '" + bloodType + "'";
                        int j = gc.PostUpdateDeleteData_Database(query2);
                        //success
                        if(j == 1){ return Request.CreateResponse(HttpStatusCode.OK, 1); }
                        //Failed
                        else { return Request.CreateResponse(HttpStatusCode.OK, 0); }
                    }
                    else
                    {
                        // failed
                        return Request.CreateResponse(HttpStatusCode.OK, 0);
                    }
                }
                else {
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
