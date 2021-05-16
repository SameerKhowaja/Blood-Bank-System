using BloodBank1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        HttpClient httpClient = new HttpClient();
        string basePath = "https://localhost:44353/";

        // 200 OK
        // 400 Bad Request
        // 500 Internal Server Error

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            // IF session not exist i.e. null go to Login View ELSE go to Dashboard View
            string check_Session = HttpContext.Session.GetString("Login_Session");
            if(check_Session == null || check_Session == "")
                return View();
            else
                return RedirectToAction("Index", "Dashboard");
        }

        public dynamic LoginCheckSuccess(Administration admin)
        {
            try
            {
                httpClient.BaseAddress = new Uri(basePath);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP POST
                var stringContent = new StringContent(JsonConvert.SerializeObject(admin), Encoding.UTF8, "application/json");
                var postTask = httpClient.PostAsync(basePath + "getLoginConfirmation", stringContent);

                var result = postTask.Result;

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    // set session if login pass
                    HttpContext.Session.SetString("Login_Session", admin.email_id);
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    TempData["errorMsg"] = "Invalid Username or Password";
                    return RedirectToAction("Login", "Home");
                }
            }
            catch
            {
                TempData["errorMsg"] = "Internal Server Error";
                return RedirectToAction("Login", "Home"); ;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
