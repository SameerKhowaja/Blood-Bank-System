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
using Newtonsoft.Json.Linq;

namespace BloodBank_API.Controllers
{
    public class RequestorController : ApiController
    {
        static string conStr = ConfigurationManager.ConnectionStrings["connectDB"].ConnectionString;
        SqlConnection con = new SqlConnection(conStr);
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
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dt.Columns.Remove("blood_id1");
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

        // POST ADD New Requestor who want blood from donorDonatedBlood_table and add data to requestor_table and requestor_donor_table
        [Route("newBloodRequestor")]
        [HttpPost]
        public HttpResponseMessage newBloodRequestor(requestor_table requestorTB)
        {
            try {
                string query = "INSERT INTO requestor_table (fullname, dob, gender, contact_number, city, blood_id, date_recieved) VALUES ('" + requestorTB.fullname + "', '" + requestorTB.dob + "', " +
                    " '" + requestorTB.gender + "', '" + requestorTB.contact_number + "', '" + requestorTB.city + "', '" + requestorTB.blood_id + "', '" + requestorTB.date_recieved + "')";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i == 1)
                {
                    // On success add to requestor_table mark CheckDonate to 1 (i=1) from donorDonatedBlood_table
                    string query2 = "UPDATE donorDonatedBlood_table SET CheckDonate = '" + i + "' WHERE blood_id = '" + requestorTB.blood_id + "'";
                    SqlCommand cmd2 = new SqlCommand(query2, con);
                    cmd2.CommandType = CommandType.Text;
                    con.Open();
                    int j = cmd2.ExecuteNonQuery();
                    con.Close();

                    if (j == 1)
                    {
                        // On success add donor_id and request_id to requestor_donor_table
                        string query3 = "SELECT donorDonatedBlood_table.donor_id, requestor_table.request_id " +
                            "FROM donorDonatedBlood_table inner join requestor_table " +
                            "ON donorDonatedBlood_table.blood_id = requestor_table.blood_id";
                        SqlDataAdapter da = new SqlDataAdapter(query3, con);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gc.TrimDataTableRow(dt);

                        if (dt.Rows.Count > 0)
                        {
                            // On success get donor_id adding donor_id and request_id to requestor_donor_table
                            string jsonDT = JsonConvert.SerializeObject(dt);
                            dynamic jsonObject = JsonConvert.DeserializeObject(jsonDT);
                            int donorID = jsonObject[0]["donor_id"];
                            int requestID = jsonObject[0]["request_id"];

                            string query4 = "INSERT INTO requestor_donor_table (donor_id, request_id) VALUES ('" + donorID + "', '" + requestID + "')";
                            SqlCommand cmd3 = new SqlCommand(query4, con);
                            cmd3.CommandType = CommandType.Text;
                            con.Open();
                            int k = cmd3.ExecuteNonQuery();
                            con.Close();

                            // success
                            if (k == 1)
                            {
                                // On Success now Update bloodbankStock_table
                                string query5 = "SELECT blood_type, unit_of_blood FROM donorDonatedBlood_table WHERE blood_id = '" + requestorTB.blood_id + "'";
                                SqlDataAdapter da2 = new SqlDataAdapter(query5, con);
                                DataTable dt2 = new DataTable();
                                da2.Fill(dt2);
                                gc.TrimDataTableRow(dt2);

                                if (dt2.Rows.Count > 0)
                                {
                                    // On success get blood_type and unit_of_blood from donorDonatedBlood_table
                                    string jsonDT2 = JsonConvert.SerializeObject(dt2);
                                    dynamic jsonObject2 = JsonConvert.DeserializeObject(jsonDT2);
                                    string bloodType = jsonObject2[0]["blood_type"];
                                    int unitBlood = jsonObject2[0]["unit_of_blood"];

                                    // Now Update bloodbankStock_table
                                    string query6 = "UPDATE bloodbankStock_table SET total_units = total_units - '" + unitBlood + "' WHERE blood_type = '" + bloodType + "'";
                                    SqlCommand cmd4 = new SqlCommand(query6, con);
                                    cmd4.CommandType = CommandType.Text;
                                    con.Open();
                                    int L = cmd4.ExecuteNonQuery();
                                    con.Close();

                                    // success
                                    if (k == 1)
                                        return Request.CreateResponse(HttpStatusCode.OK, 1);
                                    //Failed
                                    else
                                        return Request.CreateResponse(HttpStatusCode.OK, 0);
                                }
                                else
                                {
                                    // Failed
                                    return Request.CreateResponse(HttpStatusCode.OK, 0);
                                }
                            }
                            else
                            {
                                // failed
                                return Request.CreateResponse(HttpStatusCode.OK, 0);
                            }
                        }
                        else
                        {
                            // failed 0 Rows returned
                            return Request.CreateResponse(HttpStatusCode.OK, 0);
                        }
                    }        
                    else
                    {
                        // failed
                        return Request.CreateResponse(HttpStatusCode.OK, 0);
                    }
                }
                else
                {
                    // failed
                    return Request.CreateResponse(HttpStatusCode.OK, 0);
                }
            }
            catch {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.OK, -1);
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
