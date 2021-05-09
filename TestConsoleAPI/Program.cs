using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Data;
using System.Configuration;

namespace TestConsoleAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(ConfigurationManager.AppSettings.Get("apiPath") + "getDonor");
            Console.WriteLine(html.Result);
            Console.ReadKey();
        }
    }
}
