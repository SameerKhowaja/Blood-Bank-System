using BloodBank1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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
using System.Net;

namespace BloodBank1.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        HttpClient httpClient = new HttpClient();
        string basePath = "https://localhost:44353/";

        // 200 OK
        // 400 Bad Request
        // 500 Internal Server Error

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }

        private string GetSession()
        {
            string check_Session = HttpContext.Session.GetString("Login_Session");
            return check_Session;
        }

        // Dashboard Home Page
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
                    return RedirectToAction("ErrorPage", "Dashboard");
                }
            }
        }

        // Logout and Clear Session
        public IActionResult Logout()
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
                    HttpContext.Session.Remove("Login_Session");
                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    return RedirectToAction("ErrorPage", "Dashboard");
                }
            }
        }

        // Error Page View
        public IActionResult ErrorPage() => View();

        // New Donor Page View
        public IActionResult NewDonor()
        {
            string check_Session = GetSession();
            if (check_Session == null || check_Session == "")
            {
                TempData["errorMsg"] = "Unauthorized Login to Continue";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View();
            }
        }

        // Add new Donor on Submit from NewDonor() View
        [HttpPost]
        public IActionResult NewDonorAdd(Donor donor)
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
                    httpClient.BaseAddress = new Uri(basePath);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //HTTP POST
                    var stringContent = new StringContent(JsonConvert.SerializeObject(donor), Encoding.UTF8, "application/json");
                    var postTask = httpClient.PostAsync(basePath + "addNewDonor", stringContent);

                    var result = postTask.Result;
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        TempData["msgHead"] = "Success";
                        TempData["msgBody"] = "Added New Donor Data to BoodBank...!";
                        return RedirectToAction("NewDonor", "Dashboard");
                    }
                    else
                    {
                        TempData["msgHead"] = "Failed";
                        TempData["msgBody"] = "Unable to Add New Donor Data...!";
                        return RedirectToAction("NewDonor", "Dashboard");
                    }
                }
                catch
                {
                    return RedirectToAction("ErrorPage", "Dashboard");
                }
            }
        }

        // Donor Information View
        [HttpGet]
        public IActionResult DonorInfo(int id)
        {
            string check_Session = GetSession();
            if (check_Session == null || check_Session == "")
            {
                TempData["errorMsg"] = "Unauthorized Login to Continue";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                // DonorUpdate Failed then return 0
                if (id == 0)
                {
                    return RedirectToAction("ErrorPage", "Dashboard");
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
                    return RedirectToAction("ErrorPage", "Dashboard");
                }
            }
        }

        // Update Donor on Submit from DonorInfo() View
        [HttpPost]
        public IActionResult DonorUpdate(int id, Donor donor)
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
                    httpClient.BaseAddress = new Uri(basePath);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //HTTP PUT
                    donor.donor_id = id; // Set Donor id
                    var stringContent = new StringContent(JsonConvert.SerializeObject(donor), Encoding.UTF8, "application/json");
                    var postTask = httpClient.PutAsync(basePath + "editDonor", stringContent);

                    var result = postTask.Result;
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        TempData["msgHead"] = "Success";
                        TempData["msgBody"] = "Updated Donor Data to BoodBank...!";
                        return RedirectToAction("DonorInfo", "Dashboard", new { id = donor.donor_id });
                    }
                    else
                    {
                        TempData["msgHead"] = "Failed";
                        TempData["msgBody"] = "Unable to Update Donor Data...!";
                        return RedirectToAction("DonorInfo", "Dashboard", new { id = 0 });
                    }
                }
                catch
                {
                    return RedirectToAction("ErrorPage", "Dashboard");
                }
            }
        }

        // Delete Donor Data by ID from DonorInfo() View
        public IActionResult DonorDelete(int id)
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
                    httpClient.BaseAddress = new Uri(basePath);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //HTTP DELETE
                    var postTask = httpClient.DeleteAsync(basePath + "deleteDonor/" + id.ToString());

                    var result = postTask.Result;
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        TempData["msgHead"] = "Success";
                        TempData["msgBody"] = "Deleted Donor Data from BoodBank...!";
                        return RedirectToAction("BloodDonors", "Dashboard");
                    }
                    else
                    {
                        TempData["msgHead"] = "Failed";
                        TempData["msgBody"] = "Unable to Delete Donor Data...!";
                        return RedirectToAction("DonorInfo", "Dashboard", new { id = id });
                    }
                }
                catch
                {
                    return RedirectToAction("ErrorPage", "Dashboard");
                }
            }            
        }

        // Add Donor Donatating Blood from BloodDonors() View
        public IActionResult DonorDonateBlood(int id, DateTime expiry_date, string unit_of_blood)
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
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        TempData["msgHead"] = "Success";
                        TempData["msgBody"] = "Added Blood Donation of Donor to BoodBank...!";
                        return RedirectToAction("BloodDonors", "Dashboard");
                    }
                    else
                    {
                        TempData["msgHead"] = "Failed";
                        TempData["msgBody"] = "Unable to Add Blood Donation of Donor...!";
                        return RedirectToAction("BloodDonors", "Dashboard");
                    }
                }
                catch
                {
                    return RedirectToAction("ErrorPage", "Dashboard");
                }
            }
        }

        // Donor History Page to View Donor and there blood donations from BloodDonors() View
        [HttpGet]
        public IActionResult BloodDonorHistory(int id)
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
                    return RedirectToAction("ErrorPage", "Dashboard");
                }

                long Total_Unit_Donated = 0;
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

                            Total_Unit_Donated += (int)obj.unit_of_blood;
                        }
                    }
                    ViewData["total_unit_donated"] = Total_Unit_Donated;

                    return View(bloodList);
                }
                catch
                {
                    bloodList.Clear();
                    bloodList.Add(new DonorDonatedBlood() { blood_id = 0 });
                    return View(bloodList);
                }
            }
        }

        // Delete Blood Data of Donated Donor from BloodDonorHistory() View
        public IActionResult DonorBloodDataDelete(int id)
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
                    // GET Donor Data by blood ID
                    var donorData = httpClient.GetStringAsync(basePath + "getDonorIdByBloodId/" + id.ToString()).Result;
                    dynamic jsonObj = JsonConvert.DeserializeObject(donorData);
                    try
                    {
                        int donor_id = (int)jsonObj[0]["donor_id"];

                        //HTTP DELETE
                        var postTask = httpClient.DeleteAsync(basePath + "deleteBloodData/" + id.ToString());

                        var result = postTask.Result;
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            TempData["msgHead"] = "Success";
                            TempData["msgBody"] = "Deleted Donor Blood Data from BoodBank...!";
                            return RedirectToAction("BloodDonorHistory", "Dashboard", new { id = donor_id });
                        }
                        else
                        {
                            TempData["msgHead"] = "Failed";
                            TempData["msgBody"] = "Unable to Delete Blood Donation of Donor...!";
                            return RedirectToAction("BloodDonorHistory", "Dashboard", new { id = donor_id });
                        }
                    }
                    catch
                    {
                        return RedirectToAction("ErrorPage", "Dashboard");
                    }
                }
                catch
                {
                    return RedirectToAction("ErrorPage", "Dashboard");
                }
            }
        }

        // GET Donor Information by using blood_id
        [HttpGet]
        public dynamic GetDonorDataByBloodId(int id)
        {
            string check_Session = GetSession();
            if (check_Session == null || check_Session == "")
            {
                TempData["errorMsg"] = "Unauthorized Login to Continue";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                // Donor Data
                var donorData = httpClient.GetStringAsync(basePath + "getDonorDataByBloodID/" + id.ToString()).Result;
                return donorData;
            }
        }

        // Display all donor list to view BloodDonor Page View
        [HttpGet]
        public IActionResult BloodDonors()
        {
            string check_Session = GetSession();
            if (check_Session == null || check_Session == "")
            {
                TempData["errorMsg"] = "Unauthorized Login to Continue";
                return RedirectToAction("Login", "Home");
            }
            else
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
        }

        // Display all Blood Details
        [HttpGet]
        public IActionResult BloodRequest()
        {
            string check_Session = GetSession();
            if (check_Session == null || check_Session == "")
            {
                TempData["errorMsg"] = "Unauthorized Login to Continue";
                return RedirectToAction("Login", "Home");
            }
            else
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
        }

        // Delete Available Blood Data of Donated Donor from BloodRequest() View
        public IActionResult BloodDataDelete(int id)
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
                    httpClient.BaseAddress = new Uri(basePath);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //HTTP DELETE
                    var postTask = httpClient.DeleteAsync(basePath + "deleteBloodData/" + id.ToString());

                    var result = postTask.Result;
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        TempData["msgHead"] = "Success";
                        TempData["msgBody"] = "Deleted Donor Blood Data from BoodBank...!";
                        return RedirectToAction("BloodRequest", "Dashboard");
                    }
                    else
                    {
                        TempData["msgHead"] = "Failed";
                        TempData["msgBody"] = "Unable to Delete Blood Donation of Donor...!";
                        return RedirectToAction("BloodRequest", "Dashboard");
                    }
                }
                catch
                {
                    return RedirectToAction("ErrorPage", "Dashboard");
                }
            }
        }

        // Blood Expired View
        [HttpGet]
        public IActionResult BloodExpired()
        {
            string check_Session = GetSession();
            if (check_Session == null || check_Session == "")
            {
                TempData["errorMsg"] = "Unauthorized Login to Continue";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                List<DonorDonatedBlood> bloodList = new List<DonorDonatedBlood>();
                try
                {
                    var dataFetched = httpClient.GetStringAsync(basePath + "getAllBloodDetail").Result;
                    dynamic jsonObject = JsonConvert.DeserializeObject(dataFetched);

                    DateTime curr_dt = DateTime.Now;
                    foreach (var obj in jsonObject)
                    {
                        if (curr_dt > (DateTime)obj.expiry_date)    // means expired blood
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
        }

        // Delete Expired Blood Data of Donated Donor from BloodExpired() View
        public IActionResult ExpiredBloodDataDelete(int id)
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
                    httpClient.BaseAddress = new Uri(basePath);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //HTTP DELETE
                    var postTask = httpClient.DeleteAsync(basePath + "deleteExpiredBloodData/" + id.ToString());

                    var result = postTask.Result;
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        TempData["msgHead"] = "Success";
                        TempData["msgBody"] = "Deleted Expired Blood Data from BoodBank...!";
                        return RedirectToAction("BloodExpired", "Dashboard");
                    }
                    else
                    {
                        TempData["msgHead"] = "Failed";
                        TempData["msgBody"] = "Unable to Delete Expired Blood Data...!";
                        return RedirectToAction("BloodExpired", "Dashboard");
                    }
                }
                catch
                {
                    return RedirectToAction("ErrorPage", "Dashboard");
                }
            }
        }

        // Delete Reciever Data by ID from BloodRecievers() View
        public IActionResult RequestorDelete(int id)
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
                    httpClient.BaseAddress = new Uri(basePath);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //HTTP DELETE
                    var postTask = httpClient.DeleteAsync(basePath + "deleteRequestor/" + id.ToString());

                    var result = postTask.Result;
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        TempData["msgHead"] = "Success";
                        TempData["msgBody"] = "Deleted Blood Reciever Data from BoodBank...!";
                        return RedirectToAction("BloodRecievers", "Dashboard");
                    }
                    else
                    {
                        TempData["msgHead"] = "Failed";
                        TempData["msgBody"] = "Unable to Delete Blood Reciever Data...!";
                        return RedirectToAction("BloodRecievers", "Dashboard");
                    }
                }
                catch
                {
                    return RedirectToAction("ErrorPage", "Dashboard");
                }
            }
        }

        // Display BloodDetail View contain Available, Consumed and Expired Blood (type and unit)
        [HttpGet]
        public IActionResult BloodDetail()
        {
            string check_Session = GetSession();
            if (check_Session == null || check_Session == "")
            {
                TempData["errorMsg"] = "Unauthorized Login to Continue";
                return RedirectToAction("Login", "Home");
            }
            else
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
        }

        // Display all Requestor data to BloodRequest View Page
        [HttpGet]
        public IActionResult BloodRecievers()
        {
            string check_Session = GetSession();
            if (check_Session == null || check_Session == "")
            {
                TempData["errorMsg"] = "Unauthorized Login to Continue";
                return RedirectToAction("Login", "Home");
            }
            else
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
        }

        // POST new Requestor and Donate Blood from BloodRequest() View
        public IActionResult BloodRequestorAdd(int id, Requestor requestor)
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
                    requestor.blood_id = id; // set Blood ID
                    DateTime date_recieved = DateTime.Now;
                    requestor.date_recieved = date_recieved; // set current date

                    httpClient.BaseAddress = new Uri(basePath);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //HTTP POST
                    var stringContent = new StringContent(JsonConvert.SerializeObject(requestor), Encoding.UTF8, "application/json");
                    var postTask = httpClient.PostAsync(basePath + "newBloodRequestor", stringContent);

                    var result = postTask.Result;
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        TempData["msgHead"] = "Success";
                        TempData["msgBody"] = "Donated Blood to Reciever from BoodBank...!";
                        return RedirectToAction("BloodRecievers", "Dashboard");
                    }
                    else
                    {
                        TempData["msgHead"] = "Failed";
                        TempData["msgBody"] = "Unable to Donate Blood to Reciever Data...!";
                        return RedirectToAction("BloodRecievers", "Dashboard");
                    }
                }
                catch
                {
                    return RedirectToAction("ErrorPage", "Dashboard");
                }
            }
        }

        // GET All Donation Details
        [HttpGet]
        public IActionResult DonationDetail()
        {
            string check_Session = GetSession();
            if (check_Session == null || check_Session == "")
            {
                TempData["errorMsg"] = "Unauthorized Login to Continue";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                List<DonationDetail> donationList = new List<DonationDetail>();
                try
                {
                    var dataFetched = httpClient.GetStringAsync(basePath + "getAllDonationDetail").Result;
                    dynamic jsonObject = JsonConvert.DeserializeObject(dataFetched);

                    foreach (var obj in jsonObject)
                        donationList.Add(new DonationDetail()
                        {
                            r_id = (int)obj.r_id,
                            r_fullname = (string)obj.r_fullname,
                            r_date_recieved = (DateTime)obj.r_date_recieved,
                            blood_type = (string)obj.blood_type,
                            unit_of_blood = (int)obj.unit_of_blood,
                            d_date_donated = (DateTime)obj.d_date_donated,
                            d_firstname = (string)obj.d_firstname,
                            d_lastname = (string)obj.d_lastname
                        });

                    return View(donationList);
                }
                catch
                {
                    donationList.Clear();
                    donationList.Add(new DonationDetail() { r_id = 0 });
                    return View(donationList);
                }
            }
        }

        // GET Donation Detail Partial Get of Selected Row by r_id
        [HttpGet]
        public dynamic GetDonationDetailById(int id)
        {
            string check_Session = GetSession();
            if (check_Session == null || check_Session == "")
            {
                TempData["errorMsg"] = "Unauthorized Login to Continue";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                // Donation Detail Data
                var donorData = httpClient.GetStringAsync(basePath + "getDonationDetailById/" + id.ToString()).Result;
                return donorData;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            string check_Session = GetSession();
            if (check_Session == null || check_Session == "")
            {
                TempData["errorMsg"] = "Unauthorized Login to Continue";
                return RedirectToAction("Login", "Home");
            }
            else
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
