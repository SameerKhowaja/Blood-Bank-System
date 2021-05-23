using BloodBank1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BloodBank1.Controllers
{
    public class BankBloodController : Controller
    {
        private readonly ILogger<BankBloodController> _logger;
        HttpClient httpClient = new HttpClient();
        string basePath = "https://localhost:44353/";

        // 200 OK
        // 400 Bad Request
        // 500 Internal Server Error

        public BankBloodController(ILogger<BankBloodController> logger)
        {
            _logger = logger;
        }

        private string GetSession()
        {
            string check_Session = HttpContext.Session.GetString("Login_Session");
            return check_Session;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string check_Session = GetSession();
            if (check_Session == null || check_Session == "")
            {
                TempData["errorMsg"] = "Unauthorized Login to Continue";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                try
                {
                    var dataFetched = httpClient.GetStringAsync(basePath + "getBank2BloodStock").Result;
                    dynamic jsonObject = JsonConvert.DeserializeObject(dataFetched);

                    long total_blood = 0;
                    foreach (var obj in jsonObject)
                    {
                        string blood_type = obj.bloodType;
                        total_blood += (int)obj.units;

                        ViewData[blood_type] = obj.units;
                    }
                    ViewData["total_blood"] = total_blood;

                    return View();
                }
                catch
                {
                    return RedirectToAction("ErrorPage", "Dashboard");
                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
