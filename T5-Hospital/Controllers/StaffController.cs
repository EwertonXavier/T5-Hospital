using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using T5_Hospital.Models;
using System.Web.Script.Serialization;
using System.Net.Http;

namespace T5_Hospital.Controllers
{
    public class StaffController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static StaffController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }


        // GET: Staff
        public ActionResult List()
        {
            string url = "StaffData/ListStaff";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<StaffDto> staffs = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;
            return View(staffs);
        }

        // GET: Staff/Details/5
        public ActionResult Details(int id)
        { 
            string url = "StaffData/FindStaff/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StaffDto staff = response.Content.ReadAsAsync <StaffDto>().Result;

            return View(staff);
        }

        // GET: Staff/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Staff/Create
        [HttpPost]
        public ActionResult Create(Staff staff)
        {
            string url = "StaffData/AddStaff";

            string jsonpayload = jss.Serialize(staff);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return View("Error");

            }
        }

        // GET: Staff/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "StaffData/FindStaff/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StaffDto staffdto = response.Content.ReadAsAsync<StaffDto>().Result;
            return View(staffdto);
        }

        // POST: Staff/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Staff staff)
        {
            string url = "StaffData/UpdateStaff/" + id;

            string jsonpayload = jss.Serialize(staff);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if(response.IsSuccessStatusCode)
            {
                // TODO: Add update logic here

                return RedirectToAction("Details/"+id);
            }else
            {
                return View("Error");
            }
        }

        // GET: Staff/Remove/5
        public ActionResult Remove(int id)
        {
            string url = "StaffData/FindStaff/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StaffDto staff = response.Content.ReadAsAsync<StaffDto>().Result;
            return View(staff);
        }

        // POST: Staff/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "StaffData/DeleteStaff/" + id;
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
    }
}
