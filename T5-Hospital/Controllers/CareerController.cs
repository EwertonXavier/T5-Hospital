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
    public class CareerController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CareerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }

        // GET: Career
        [Authorize]
        public ActionResult List(string CareerSearch = null)
        {
            string url = "CareerData/ListCareers";

            if (CareerSearch != null)
                url += $"?CareerSearch={CareerSearch}";

            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CareerDto> careers = response.Content.ReadAsAsync<IEnumerable<CareerDto>>().Result;
            return View(careers);
        }

        // GET: Career/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            string url = "CareerData/FindCareer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CareerDto department = response.Content.ReadAsAsync<CareerDto>().Result;
            return View(department);
        }

        // GET: Career/Create
        [Authorize]
        public ActionResult New()
        {
            string url = "DepartmentData/ListDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            return View(departments);
        }

        // POST: Career/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Career career)
        {
            string url = "CareerData/AddCareer";


            string jsonpayload = jss.Serialize(career);

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

        // GET: Career/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            UpdateCareer updateCareer = new UpdateCareer();
            string careerUrl = "CareerData/FindCareer/" + id;
            HttpResponseMessage response = client.GetAsync(careerUrl).Result;
            updateCareer.SelectedCareer = response.Content.ReadAsAsync<CareerDto>().Result;
            string deptUrl = "DepartmentData/ListDepartments";
            response = client.GetAsync(deptUrl).Result;
            updateCareer.DepartmentList = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            return View(updateCareer);
        }

        // POST: Career/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, Career career)
        {
            string url = "CareerData/UpdateCareer/" + id;

            string jsonPayload = jss.Serialize(career);
            HttpContent content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details/" + id);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Career/Remove/5
        [Authorize]
        public ActionResult Remove(int id)
        {
            string url = "CareerData/FindCareer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CareerDto careerDto = response.Content.ReadAsAsync<CareerDto>().Result;
            return View(careerDto);
        }

        // POST: Career/Delete/5
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "CareerData/DeleteCareer/" + id;
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
        public ActionResult Error()
        {
            return View();
        }
    }
}
