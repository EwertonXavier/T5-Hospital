using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using T5_Hospital.Models;
using T5_Hospital.Models.ViewModels;

namespace T5_Hospital.Controllers
{
    public class DonationController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DonationController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }

        [Authorize]
        // GET: Donation/List
        public ActionResult List(string DonationSearch = null)
        {
            //objective: communicate with Donation api to retrieve a list of Donations

            string url = "DonationData/ListDonations";

            if (DonationSearch != null)
            url += "?DonationSearch=" + DonationSearch;


            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DonationDto> donations = response.Content.ReadAsAsync<IEnumerable<DonationDto>>().Result;
            return View(donations);
        }

        [Authorize]
        // GET: Donation/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our donation data api to retrieve one donation

            string url = "DonationData/FindDonation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonationDto SelectedDonation = response.Content.ReadAsAsync<DonationDto>().Result;

            return View(SelectedDonation);

        }

        [Authorize]
        // GET: Donation/New
        public ActionResult New()
        {
            NewDonation ViewModel = new NewDonation();

            //information about all the donors in the system
            

           string url = "donordata/listdonors/";
           HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DonorDto> DonorOptions = response.Content.ReadAsAsync<IEnumerable<DonorDto>>().Result;
            ViewModel.DonorOptions = DonorOptions;

            url = "departmentdata/listdepartments/";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            ViewModel.DepartmentOptions = DepartmentOptions;

            return View(ViewModel);
        }

        [Authorize]
        // POST: Donation/Create
        [HttpPost]
        public ActionResult Create(Donation donation)
        {
            //objective: add a new donation into our system using the API

            string url = "DonationData/AddDonation";

            string jsonpayload = jss.Serialize(donation);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [Authorize]
        // GET: Donation/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateDonation ViewModel = new UpdateDonation();

            string url = "DonationData/FindDonation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ViewModel.SelectedDonation = response.Content.ReadAsAsync<DonationDto>().Result;
          

            //all donors to choose from when updating this Donation
            url = "DonorData/ListDonors/";
            response = client.GetAsync(url).Result;
            ViewModel.DonorOptions = response.Content.ReadAsAsync<IEnumerable<DonorDto>>().Result;

            url = "DepartmentData/ListDepartments/";
            response = client.GetAsync(url).Result;
            ViewModel.DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            

            return View(ViewModel);
        }

        [Authorize]
        // POST: Donation/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, DonationDto donation)
        {
            string url = "DonationData/UpdateDonation/" + id;


            string jsonpayload = jss.Serialize(donation);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details/" + id);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [Authorize]
        // POST: Donation/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "DonationData/DeleteDonation/" + id;
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [Authorize]
        // GET: Donation/Remove/5
        public ActionResult Remove(int id)
        {
            string url = "DonationData/FindDonation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonationDto SelectedDonation = response.Content.ReadAsAsync<DonationDto>().Result;
            return View(SelectedDonation);
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
