using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace BloodBank_API.Controllers
{
    public class GenericClass
    {
        // 200 OK
        // 400 Bad Request
        // 500 Internal Server Error

        static string conStr = ConfigurationManager.ConnectionStrings["connectDB"].ConnectionString;
        SqlConnection con = new SqlConnection(conStr);

        // Trim Data Table Row
        public void TrimDataTableRow(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows) // trim string data
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.DataType == typeof(string))
                    {
                        object o = dr[dc];
                        if (!Convert.IsDBNull(o) && o != null)
                        {
                            dr[dc] = o.ToString().Trim();
                        }
                    }
                }
            }
        }

        // GET Data From Database
        public DataTable GetData_Database(string query)
        {
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            TrimDataTableRow(dt);   // trim datatable
            return dt;
        }

        // POST, Update, Delete Data to Database
        public int PostUpdateDeleteData_Database(string query)
        {
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

    }
}