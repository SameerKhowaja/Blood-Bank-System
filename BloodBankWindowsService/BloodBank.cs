using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBankWindowsService
{
    class BloodBank
    {
        public static DataTable GetAllDataDB2()
        {
            string query = @"select * from BloodBank2View";
            DBHelper dbh = new DBHelper("db2");
            return dbh.GetDataTable(query);
        }
        public static DataTable GetAllDataDB1()
        {
            string query = @"select * from bloodbank1View";
            DBHelper dbh = new DBHelper("db1");
            return dbh.GetDataTable(query);
        }
    }
}
