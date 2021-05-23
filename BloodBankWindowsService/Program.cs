using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBankWindowsService
{
    class Program
    {
        static void Main(string[] args)
        {


            WindowService.waitTime = Convert.ToInt32(ConfigurationManager.AppSettings["setTimeInMinutes"]);
            WindowService.MyAction = () => MyFunction(ConfigurationManager.AppSettings["Path"], "bloodbank1", "BloodBank2");
            Environment.ExitCode = Host.RunWindowsService("BLOODBANK", "BLOODBANK", "BLOODBANK service with a timer period " + WindowService.waitTime + " min", "BLOODBANK");
            //MyFunction(ConfigurationManager.AppSettings["Path"],"bloodbank1","BloodBank2");
        }

        private static void MyFunction(string filePath,string db1,string db2)
        {
            #region
            // string json = File.ReadAllText(@"D:\path.json");
            // DataTable oldData = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));

            /*var tables = new[] { oldData, newData };
            var combined = CombineDataTables(tables);*/
            #endregion

            var newDatadb1 = BloodBank.GetAllDataDB1();
            SaveData(newDatadb1, $@"{filePath}\{db1}.json");
            var newDatadb2 = BloodBank.GetAllDataDB2();
            SaveData(newDatadb2, $@"{filePath}\{db2}.json");



        }
        public static void SaveData(DataTable table, string file)
        {
            string allDataJSon = JsonConvert.SerializeObject(table);
            File.WriteAllText(file, allDataJSon);
        }
       /* public static DataTable CombineDataTables(params DataTable[] args)
        {
            return args.SelectMany(dt => dt.AsEnumerable()).CopyToDataTable();
        }*/

    }
}
