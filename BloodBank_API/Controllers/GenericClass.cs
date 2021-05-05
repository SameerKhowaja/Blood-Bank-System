using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BloodBank_API.Controllers
{
    public class GenericClass
    {
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
        // Trim Data Table Row

    }
}