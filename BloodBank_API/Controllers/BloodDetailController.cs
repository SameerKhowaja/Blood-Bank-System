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
        static string conStr = ConfigurationManager.ConnectionStrings["connectDB"].ConnectionString;
        SqlConnection con = new SqlConnection(conStr);
        GenericClass gc = new GenericClass();

        // GET All Blood Detail (Available or Expired or Consumed) from donorDonatedBlood_table with donors_table information
        [Route("getAllBloodWithDonor")]
        [HttpGet]
        public HttpResponseMessage getAllBloodWithDonor()
        {
            string query = "SELECT donorDonatedBlood_table.*, donors_table.* " +
                "FROM donorDonatedBlood_table inner join donors_table " +
                "ON donorDonatedBlood_table.donor_id = donors_table.donor_id";
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dt.Columns.Remove("donor_id1");
            gc.TrimDataTableRow(dt);

            if (dt.Rows.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, dt);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, 0);
            }
        }

        // GET All Blood Details (Available or Expired or Consumed) from donorDonatedBlood_table
        [Route("getAllBloodDetail")]
        [HttpGet]
        public HttpResponseMessage getAllBloodDetail()
        {
            string query = "SELECT * FROM donorDonatedBlood_table";
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gc.TrimDataTableRow(dt);

            if (dt.Rows.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, dt);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, 0);
            }
        }

    }
}
