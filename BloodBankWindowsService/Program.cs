using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBankWindowsService
{
    class Program
    {
        static void Main(string[] args)
        {

           // WindowService.waitTime = Convert.ToInt32(ConfigurationManager.AppSettings["setTimeInMinutes"]);
           // WindowService.MyAction = () => MyFunction();
            //Environment.ExitCode = Host.RunWindowsService("BLOODBANK", "BLOODBANK", "BLOODBANK service with a timer period " + WindowService.waitTime + " min", "BLOODBANK");
            MyFunction();
        }

        private static void MyFunction()
        { 
            //implement logic here
        }
    }
}
