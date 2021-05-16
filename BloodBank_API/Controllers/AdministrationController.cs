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
    public class AdministrationController : ApiController
    {
        GenericClass gc = new GenericClass();

        // GET Login Confirmation
        [Route("getLoginConfirmation")]
        [HttpPost]
        public HttpResponseMessage getLoginConfirmation(administration_table admin)
        {
            try
            {
                string query = "SELECT email_id from administration_table WHERE email_id = '" + admin.email_id + "' AND password = '" + admin.password + "'";
                DataTable dt = gc.GetData_Database(query);
                if (dt.Rows.Count > 0) { return Request.CreateResponse(HttpStatusCode.OK, 1); }
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
