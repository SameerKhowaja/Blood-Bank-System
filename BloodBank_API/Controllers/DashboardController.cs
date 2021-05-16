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
    public class DashboardController : ApiController
    {
        GenericClass gc = new GenericClass();

        // Need to Update bloodbankStock_table every day (If blood expired then remove from bloodbankStock_table)
        public HttpStatusCode updateStockTable()
        {
            try
            {
                string[] bloodType = { "O+", "O-", "A+", "A-", "B+", "B-", "AB+", "AB-" };
                string query1 = "SELECT blood_type, ISNULL(SUM(unit_of_blood), 0) From donorDonatedBlood_table " +
                        "WHERE CheckDonate = 0 AND expiry_date >= CURRENT_TIMESTAMP GROUP BY blood_type";
                DataTable dt = gc.GetData_Database(query1);
                if (dt.Rows.Count <= 0)
                {
                    foreach (string type in bloodType)
                    {
                        dt.Rows.Add(type, 0);
                    }
                }
                else
                {
                    foreach (string type in bloodType)
                    {
                        DataRow[] rows = dt.Select("blood_type='" + type + "'");
                        if (rows.Length == 0)
                        {
                            dt.Rows.Add(type, 0);
                        }
                    }
                }

                // Updated Stock Data All blood groups // NHI CHALA GA
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string query2 = "UPDATE bloodbankStock_table SET total_units = '" + row.Field<int>(1) + "' WHERE blood_type = '" + row.Field<string>(0) + "'";
                    i += gc.PostUpdateDeleteData_Database(query2);
                }
                
                if(i >= 0)
                {
                    return HttpStatusCode.OK;
                }
                else
                {
                    // Error occured
                    return HttpStatusCode.BadRequest;
                }
            }
            catch
            {
                // Error occured
                return HttpStatusCode.InternalServerError;
            }
        }

        // GET All Bloodbank Stock from bloodbankStock_table
        [Route("getBloodStock")]
        [HttpGet]
        public HttpResponseMessage getBloodStock()
        {
            try
            {
                HttpStatusCode code = updateStockTable();
                if(code == HttpStatusCode.OK)
                {
                    string query = "SELECT * from bloodbankStock_table";
                    DataTable dt = gc.GetData_Database(query);
                    if (dt.Rows.Count > 0) { return Request.CreateResponse(HttpStatusCode.OK, dt); }
                    else { return Request.CreateResponse(HttpStatusCode.BadRequest, 0); }
                }
                else
                {
                    // Error occured
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, -1);
                }
            }
            catch
            {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.InternalServerError, -1);
            }
        }

    }
}
