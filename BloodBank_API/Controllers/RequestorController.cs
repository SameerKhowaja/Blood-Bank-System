﻿using System;
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
using Newtonsoft.Json.Linq;

namespace BloodBank_API.Controllers
{
    public class RequestorController : ApiController
    {
        GenericClass gc = new GenericClass();

        //GET All Blood Requestor Data from requestor_table join donorDonatedBlood_table on blood_id
        [Route("getRequestor")]
        [HttpGet]
        public HttpResponseMessage getAllRequestor()
        {
            try 
            {
                string query = "SELECT requestor_table.*, donorDonatedBlood_table.* " +
                    "FROM requestor_table inner join donorDonatedBlood_table " +
                    "ON requestor_table.blood_id = donorDonatedBlood_table.blood_id";

                DataTable dt = gc.GetData_Database(query);
                dt.Columns.Remove("blood_id1");
                if (dt.Rows.Count > 0) { return Request.CreateResponse(HttpStatusCode.OK, dt); }
                else { return Request.CreateResponse(HttpStatusCode.BadRequest, 0); }
            }
            catch
            {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.InternalServerError, -1);
            }
            
        }

        // POST ADD New Requestor who want blood from donorDonatedBlood_table and add data to requestor_table and requestor_donor_table
        [Route("newBloodRequestor")]
        [HttpPost]
        public HttpResponseMessage newBloodRequestor(requestor_table requestorTB)
        {
            try {
                string query = "INSERT INTO requestor_table (fullname, dob, gender, contact_number, city, blood_id, date_recieved) VALUES ('" + requestorTB.fullname + "', '" + requestorTB.dob + "', " +
                    " '" + requestorTB.gender + "', '" + requestorTB.contact_number + "', '" + requestorTB.city + "', '" + requestorTB.blood_id + "', '" + requestorTB.date_recieved + "')";
                int i = gc.PostUpdateDeleteData_Database(query);

                if (i == 1)
                {
                    // On success add to requestor_table mark CheckDonate to 1 (i=1) from donorDonatedBlood_table
                    string query2 = "UPDATE donorDonatedBlood_table SET CheckDonate = '" + i + "' WHERE blood_id = '" + requestorTB.blood_id + "'";
                    int j = gc.PostUpdateDeleteData_Database(query2);

                    // success
                    if (j == 1)
                    {
                        // On Success now Update bloodbankStock_table
                        string query5 = "SELECT blood_type, unit_of_blood FROM donorDonatedBlood_table WHERE blood_id = '" + requestorTB.blood_id + "'";
                        DataTable dt2 = gc.GetData_Database(query5);

                        if (dt2.Rows.Count > 0)
                        {
                            // On success get blood_type and unit_of_blood from donorDonatedBlood_table
                            string jsonDT2 = JsonConvert.SerializeObject(dt2);
                            dynamic jsonObject2 = JsonConvert.DeserializeObject(jsonDT2);
                            string bloodType = jsonObject2[0]["blood_type"];
                            int unitBlood = jsonObject2[0]["unit_of_blood"];

                            // Now Update bloodbankStock_table
                            string query6 = "UPDATE bloodbankStock_table SET total_units = total_units - '" + unitBlood + "' WHERE blood_type = '" + bloodType + "'";
                            int k = gc.PostUpdateDeleteData_Database(query6);

                            // success
                            if (k == 1)
                                return Request.CreateResponse(HttpStatusCode.OK, 1);
                            //Failed
                            else
                                return Request.CreateResponse(HttpStatusCode.BadRequest, 0);
                        }
                        else
                        {
                            // Failed
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
            catch {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.InternalServerError, -1);
            }
            
        }

        // DELETE Requestor Data with request_id from requestor_table
        [Route("deleteRequestor/{id}")]
        [HttpDelete]
        public HttpResponseMessage deleteRequestor(int id)
        {
            try
            {
                string query = "DELETE From requestor_table WHERE request_id = '" + id + "'";
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
