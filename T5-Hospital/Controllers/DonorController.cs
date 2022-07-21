using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using T5_Hospital.Models;

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

        // GET: Donor
        public ActionResult List()
        {
            string url = "DonorData/ListDonors";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DonorDto> donors = response.Content.ReadAsAsync<IEnumerable<DonorDto>>().Result;
            return View(donors);
        }

        // GET: Donor/Details/5
        public ActionResult Details(int id)
        {
            string url = "DonorData/FindDonor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonorDto donor = response.Content.ReadAsAsync<DonorDto>().Result;
            return View(donor);
        }

        // GET: Donor/New
        public ActionResult New()
        {
            return View();
        }

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

        // GET: Donor/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "DonorData/FindDonor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonorDto donorDto = response.Content.ReadAsAsync<DonorDto>().Result;
            return View(donorDto);
        }

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

        // GET: Donor/Remove/5
        public ActionResult Remove(int id)
        {
            string url = "DonorData/FindDonor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonorDto donorDto = response.Content.ReadAsAsync<DonorDto>().Result;
            return View(donorDto);
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
