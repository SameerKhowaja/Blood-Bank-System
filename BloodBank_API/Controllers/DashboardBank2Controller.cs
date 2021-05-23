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
    public class DashboardBank2Controller : ApiController
    {
        GenericClass gc = new GenericClass();

        HttpClient httpClient = new HttpClient();
        string BloodGroups_Path = "https://localhost:44324/api/bloodgroup";

        // GET All Bloodbank2 Stock from bloodbank2 Database BloodGroups table 
        [Route("getBank2BloodStock")]
        [HttpGet]
        public HttpResponseMessage getBank2BloodStock()
        {
            try
            {
                // On success return BloodGroups table data in JSON
                var dataFetched = httpClient.GetStringAsync(BloodGroups_Path).Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(dataFetched);

                DataTable dt = (DataTable)JsonConvert.DeserializeObject(dataFetched, (typeof(DataTable)));
                if (dt.Rows.Count > 0) { return Request.CreateResponse(HttpStatusCode.OK, dt); }    // success
                else { return Request.CreateResponse(HttpStatusCode.BadRequest, 0); }   // no record found
            }
            catch
            {
                // Error occured
                return Request.CreateResponse(HttpStatusCode.InternalServerError, -1);
            }
        }

    }
}
