using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBankWindowsService
{
    class DBHelper
    {

        SqlConnection connectionObj;
        public DBHelper(string name)
        {
            connectionObj = new SqlConnection();
            string str = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            connectionObj.ConnectionString = str;

        }

        private void ConnectionOpen()
        {
            connectionObj.Open();
        }

        private void ConnectionClose()
        {
            connectionObj.Close();
        }

        public DataTable GetDataTable(string query, DataTable param = null)
        {
            try
            {

                DataTable dt = new DataTable("Result");
                ConnectionOpen();
                SqlDataAdapter adapter = new SqlDataAdapter(query, connectionObj);
                if (param != null)
                {
                    foreach (DataRow dr in param.Rows)
                    {

                        adapter.SelectCommand.Parameters.AddWithValue("@" + dr["Key"].ToString(), dr["Value"].ToString());
                    }
                }
                adapter.Fill(dt);
                ConnectionClose();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDataTableSP(string SPName, DataTable param = null)
        {
            try
            {
                DataTable dt = new DataTable("Result");
                ConnectionOpen();
                SqlCommand command = new SqlCommand();
                command.Connection = connectionObj;
                command.CommandText = SPName;
                command.CommandType = CommandType.StoredProcedure;
                if (param != null)
                {
                    foreach (DataRow dr in param.Rows)
                    {
                        command.Parameters.AddWithValue("@" + dr["Key"].ToString(), dr["Value"].ToString());
                    }
                }

                SqlDataReader rdr = command.ExecuteReader();

                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    if (!dt.Columns.Contains(rdr.GetName(i)))
                        dt.Columns.Add(rdr.GetName(i));
                }
                while (rdr.Read())
                {
                    DataRow dr = dt.NewRow();
                    foreach (DataColumn dc in dt.Columns)
                        dr[dc.ColumnName] = rdr[dc.ColumnName];

                    dt.Rows.Add(dr);
                }
                ConnectionClose();

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

