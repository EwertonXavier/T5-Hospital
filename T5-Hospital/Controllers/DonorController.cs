using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using T5_Hospital.Models;
using T5_Hospital.Models.ViewModels;

namespace T5_Hospital.Controllers
{
    public class DonorController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DonorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }

        [Authorize]
        // GET: Donor
        public ActionResult List(string DonorSearch = null)
        {
            string url = "DonorData/ListDonors";

            if (DonorSearch != null)
           url += "?DonorSearch=" + DonorSearch;

            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DonorDto> donors = response.Content.ReadAsAsync<IEnumerable<DonorDto>>().Result;
            return View(donors);
        }

        [Authorize]
        // GET: Donor/Details/5
        public ActionResult Details(int id)
        {
            DetailsDonor ViewModel = new DetailsDonor();
            string url = "DonorData/FindDonor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ViewModel.SelectedDonor = response.Content.ReadAsAsync<DonorDto>().Result;
         

            //Showcase information about the donation related to this Donor

            url = "DonationData/FindDonationsForDonor/" + id;
            response = client.GetAsync(url).Result;
            ViewModel.RelatedDonations = response.Content.ReadAsAsync<IEnumerable<DonationDto>>().Result;
           

            return View(ViewModel);

        }

        [Authorize]
        // GET: Donor/New
        public ActionResult New()
        {
            return View();
        }

        [Authorize]
        // POST: Donor/Create
        [HttpPost]
        public ActionResult Create(Donor donor)
        {
            string url = "DonorData/AddDonor";

            string jsonpayload = jss.Serialize(donor);

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
        // GET: Donor/Edit/5
        public ActionResult Edit(int id)
        {

            string url = "DonorData/FindDonor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonorDto SelectedDonor = response.Content.ReadAsAsync<DonorDto>().Result;

            return View(SelectedDonor);
        }

        [Authorize]
        // POST: Donor/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Donor donor)
        {
            string url = "DonorData/UpdateDonor/" + id;


            string jsonpayload = jss.Serialize(donor);

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
        // POST: Donor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "DonorData/DeleteDonor/" + id;
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
        // GET: Donor/Remove/5
        public ActionResult Remove(int id)
        {
            string url = "DonorData/FindDonor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonorDto SelectedDonor = response.Content.ReadAsAsync<DonorDto>().Result;
            return View(SelectedDonor);
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
