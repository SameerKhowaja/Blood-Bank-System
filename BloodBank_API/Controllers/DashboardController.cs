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

        // GET All Bloodbank Stock from bloodbankStock_table
        [Route("getBloodStock")]
        [HttpGet]
        public HttpResponseMessage getBloodStock()
        {
            try
            {
                string query = "SELECT * from bloodbankStock_table";
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
