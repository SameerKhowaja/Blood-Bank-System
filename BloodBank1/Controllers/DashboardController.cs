using BloodBank1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net.Http.Headers;
using System.Text;

namespace BloodBank1.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        HttpClient httpClient = new HttpClient();
        string basePath = "https://localhost:44353/";

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }

        // Dashboard Home Page
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var dataFetched = httpClient.GetStringAsync(basePath + "getBloodStock").Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(dataFetched);

                long total_blood = 0;
                foreach (var obj in jsonObject)
                {
                    string blood_type = obj.blood_type;
                    total_blood += (int)obj.total_units;

                    ViewData[blood_type] = obj.total_units;
                }
                ViewData["total_blood"] = total_blood;

                return View();
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        // Message When new Donor Added or Not
        public IActionResult Message(int id)
        {
            ViewData["Message"] = id;
            return View();
        }

        // Error Page View
        public IActionResult ErrorPage()
        {
            return View();
        }

        // New Donor Page View
        public IActionResult NewDonor()
        {
            return View();
        }

        // Add new Donor on SUbmit from NewDonor View
        [HttpPost]
        public IActionResult NewDonorAdd(Donor donor)
        {
            try
            {
                httpClient.BaseAddress = new Uri(basePath);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP POST
                var stringContent = new StringContent(JsonConvert.SerializeObject(donor), Encoding.UTF8, "application/json");
                var postTask = httpClient.PostAsync(basePath + "addNewDonor", stringContent);

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Message", new { id = 1 });
                }
                else
                {
                    return RedirectToAction("Message", new { id = 0 });
                }
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        // Donor Information View
        [HttpGet]
        public IActionResult DonorInfo(int id)
        {
            // DonorUpdate Failed then return 0
            if (id == 0)
            {
                return RedirectToAction("ErrorPage");
            }

            // Else
            try
            {
                // Donor Data
                var donorData = httpClient.GetStringAsync(basePath + "getDonor/" + id.ToString()).Result;
                dynamic jsonObj = JsonConvert.DeserializeObject(donorData);

                ViewData["donor_id"] = (int)jsonObj[0]["donor_id"];
                ViewData["firstname"] = (string)jsonObj[0]["firstname"];
                ViewData["lastname"] = (string)jsonObj[0]["lastname"];
                ViewData["blood_type"] = (string)jsonObj[0]["blood_type"];
                DateTime ldd = (DateTime)jsonObj[0]["last_date_donation"];
                ViewData["last_date_donation"] = ldd.ToString("yyyy/MM/dd");
                ViewData["gender"] = (string)jsonObj[0]["gender"];
                DateTime dob = (DateTime)jsonObj[0]["dob"];
                ViewData["dob"] = dob.ToString("yyyy/MM/dd");
                ViewData["contact_number"] = (string)jsonObj[0]["contact_number"];
                ViewData["city"] = (string)jsonObj[0]["city"];
                ViewData["email_id"] = (string)jsonObj[0]["email_id"];
                ViewData["address"] = (string)jsonObj[0]["address"];

                return View();
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        // Update Donor on Submit from DonorInfo View
        [HttpPost]
        public dynamic DonorUpdate(int id, Donor donor)
        {
            try
            {
                httpClient.BaseAddress = new Uri(basePath);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP PUT
                donor.donor_id = id; // Set Donor id
                var stringContent = new StringContent(JsonConvert.SerializeObject(donor), Encoding.UTF8, "application/json");
                var postTask = httpClient.PutAsync(basePath + "editDonor", stringContent);

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("DonorInfo", new { id = donor.donor_id });
                }
                else
                {
                    return RedirectToAction("DonorInfo", new { id = 0 });
                }
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        // Delete Donor Data by ID
        public IActionResult DonorDelete(int id)
        {
            try
            {
                httpClient.BaseAddress = new Uri(basePath);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP DELETE
                var postTask = httpClient.DeleteAsync(basePath + "deleteDonor/" + id.ToString());

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("BloodDonors");
                }
                else
                {
                    return RedirectToAction("ErrorPage");
                }
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        // Donor Donatating Blood
        public dynamic DonorDonateBlood(int id, DateTime expiry_date, string unit_of_blood)
        {
            try
            {
                DonorDonatedBlood donateBlood = new DonorDonatedBlood();
                donateBlood.donor_id = id;
                donateBlood.unit_of_blood = int.Parse(unit_of_blood);
                DateTime date_donated = DateTime.Now;
                donateBlood.date_donated = date_donated;
                donateBlood.expiry_date = expiry_date;

                httpClient.BaseAddress = new Uri(basePath);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP POST
                var stringContent = new StringContent(JsonConvert.SerializeObject(donateBlood), Encoding.UTF8, "application/json");
                var postTask = httpClient.PostAsync(basePath + "addNewDonorDonation", stringContent);

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Message", new { id = 2 });
                }
                else
                {
                    return RedirectToAction("Message", new { id = 3 });
                }
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        // Donor History Page to View Donor and their donation
        [HttpGet]
        public IActionResult BloodDonorHistory(int id)
        {
            try
            {
                // Donor Data
                var donorData = httpClient.GetStringAsync(basePath + "getDonor/" + id.ToString()).Result;
                dynamic jsonObj = JsonConvert.DeserializeObject(donorData);

                ViewData["donor_id"] = (int)jsonObj[0]["donor_id"];
                ViewData["fullname"] = (string)jsonObj[0]["firstname"] + " " + (string)jsonObj[0]["lastname"];
                ViewData["blood_type"] = (string)jsonObj[0]["blood_type"];
                DateTime ldd = (DateTime)jsonObj[0]["last_date_donation"];
                ViewData["last_date_donation"] = ldd.ToString("dd/MM/yyyy");
                ViewData["gender"] = (string)jsonObj[0]["gender"];
                DateTime dob = (DateTime)jsonObj[0]["dob"];
                ViewData["dob"] = dob.ToString("dd/MM/yyyy");
                ViewData["contact_number"] = (string)jsonObj[0]["contact_number"];
                ViewData["city"] = (string)jsonObj[0]["city"];
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }

            
            List<DonorDonatedBlood> bloodList = new List<DonorDonatedBlood>();
            try
            {
                // Blood Data
                var dataFetched = httpClient.GetStringAsync(basePath + "getDonorDonation/" + id.ToString()).Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(dataFetched);

                foreach (var obj in jsonObject)
                {
                    if ((int)obj.donor_id == id)
                    {
                        bloodList.Add(new DonorDonatedBlood()
                        {
                            blood_id = (int)obj.blood_id,
                            unit_of_blood = (int)obj.unit_of_blood,
                            date_donated = (DateTime)obj.date_donated,
                            expiry_date = (DateTime)obj.expiry_date,
                            CheckDonate = (int)obj.CheckDonate
                        });
                    }
                }

                return View(bloodList);
            }
            catch
            {
                bloodList.Clear();
                bloodList.Add(new DonorDonatedBlood() { blood_id = 0 });
                return View(bloodList);
            }
        }

        // Display all donor list to view BLoodDonor Page View
        [HttpGet]
        public IActionResult BloodDonors()
        {
            List<Donor> donorList = new List<Donor>();
            try
            {
                var dataFetched = httpClient.GetStringAsync(basePath + "getDonor").Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(dataFetched);

                foreach (var obj in jsonObject)
                    donorList.Add(new Donor()
                    {
                        donor_id = (int)obj.donor_id,
                        firstname = (string)obj.firstname,
                        lastname = (string)obj.lastname,
                        dob = (DateTime)obj.dob,
                        gender = (string)obj.gender,
                        blood_type = (string)obj.blood_type,
                        last_date_donation = (DateTime)obj.last_date_donation,
                        contact_number = (string)obj.contact_number,
                        email_id = (string)obj.email_id,
                        city = (string)obj.city,
                        address = (string)obj.address
                    });

                return View(donorList);
            }
            catch
            {
                donorList.Clear();
                donorList.Add(new Donor() { donor_id = 0 });
                return View(donorList);
            }
            
        }

        // Display all Blood Details
        [HttpGet]
        public IActionResult BloodRequest()
        {
            List<DonorDonatedBlood> bloodList = new List<DonorDonatedBlood>();
            try
            {
                var dataFetched = httpClient.GetStringAsync(basePath + "getAllAvailableBloodDetail").Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(dataFetched);

                foreach (var obj in jsonObject)
                {
                    bloodList.Add(new DonorDonatedBlood()
                    {
                        blood_id = (int)obj.blood_id,
                        unit_of_blood = (int)obj.unit_of_blood,
                        date_donated = (DateTime)obj.date_donated,
                        expiry_date = (DateTime)obj.expiry_date,
                        blood_type = (string)obj.blood_type
                    });
                }

                return View(bloodList);
            }
            catch
            {
                bloodList.Clear();
                bloodList.Add(new DonorDonatedBlood() { blood_id = 0 });
                return View(bloodList);
            }
        }

        // Delete Blood Data of Donated Donor
        public IActionResult BloodDataDelete(int id)
        {
            try
            {
                httpClient.BaseAddress = new Uri(basePath);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP DELETE
                var postTask = httpClient.DeleteAsync(basePath + "deleteBloodData/" + id.ToString());

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("BloodRequest");
                }
                else
                {
                    return RedirectToAction("ErrorPage");
                }
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        // Delete Reciever Data by ID
        public IActionResult RequestorDelete(int id)
        {
            try
            {
                httpClient.BaseAddress = new Uri(basePath);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP DELETE
                var postTask = httpClient.DeleteAsync(basePath + "deleteRequestor/" + id.ToString());

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("BloodRecievers");
                }
                else
                {
                    return RedirectToAction("ErrorPage");
                }
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        // Display BloodDetail View contain Available, Consumed and Expired Blood (type and unit)
        [HttpGet]
        public IActionResult BloodDetail()
        {
            List<Dictionary<string, int>> MyBloodList = new List<Dictionary<string, int>>();

            Dictionary<string, int> availableBlood = new Dictionary<string, int>();
            Dictionary<string, int> consumeBlood = new Dictionary<string, int>();
            Dictionary<string, int> expireBlood = new Dictionary<string, int>();
            try
            {
                var dataFetched1 = httpClient.GetStringAsync(basePath + "getAllBloodData").Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(dataFetched1);

                foreach (var obj in jsonObject)
                {
                    // Available Blood
                    if ((int)obj.CheckDonate == 0 && (DateTime)obj.expiry_date >= DateTime.Now)
                    {
                        string key = (string)obj.blood_type;
                        int value = (int)obj.unit_of_blood;
                        if (!availableBlood.ContainsKey(key))
                        {
                            availableBlood.Add(key, value);
                        }
                        else
                        {
                            availableBlood[key] += value;
                        }
                    }
                    else
                    {
                        // Consumed Blood
                        if ((int)obj.CheckDonate == 1)
                        {
                            string key = (string)obj.blood_type;
                            int value = (int)obj.unit_of_blood;
                            if (!consumeBlood.ContainsKey(key))
                            {
                                consumeBlood.Add(key, value);
                            }
                            else
                            {
                                consumeBlood[key] += value;
                            }
                        }

                        // Expired Blood
                        if ((DateTime)obj.expiry_date < DateTime.Now)
                        {
                            string key = (string)obj.blood_type;
                            int value = (int)obj.unit_of_blood;
                            if (!expireBlood.ContainsKey(key))
                            {
                                expireBlood.Add(key, value);
                            }
                            else
                            {
                                expireBlood[key] += value;
                            }
                        }
                    }                  
                }

                MyBloodList.Add(availableBlood);
                MyBloodList.Add(consumeBlood);
                MyBloodList.Add(expireBlood);

                return View(MyBloodList);
            }
            catch
            {
                MyBloodList.Clear();
                return View(MyBloodList);
            }
        }

        // Display all Requestor data to BloodRequest View Page
        [HttpGet]
        public IActionResult BloodRecievers()
        {
            List<Requestor> requestor = new List<Requestor>();
            try
            {
                var dataFetched = httpClient.GetStringAsync(basePath + "getRequestor").Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(dataFetched);

                foreach (var obj in jsonObject)
                {
                    requestor.Add(new Requestor()
                    {
                        request_id = (int)obj.request_id,
                        fullname = (string)obj.fullname,
                        dob = (DateTime)obj.dob,
                        gender = (string)obj.gender,
                        contact_number = (string)obj.contact_number,
                        city = (string)obj.city,
                        date_recieved = (DateTime)obj.date_recieved,
                        blood_type = (string)obj.blood_type,
                        unit_of_blood = (int)obj.unit_of_blood
                    });
                }

                return View(requestor);
            }
            catch
            {
                requestor.Clear();
                requestor.Add(new Requestor() { request_id = 0 });
                return View(requestor);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
