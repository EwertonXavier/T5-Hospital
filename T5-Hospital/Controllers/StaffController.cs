using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using T5_Hospital.Models;
using System.Web.Script.Serialization;
using System.Net.Http;
using T5_Hospital.Models.ViewModels;

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

        [Authorize]
        // GET: Staff
        public ActionResult List(string StaffSearch = null)
        {
            string url = "StaffData/ListStaff";
            if (StaffSearch != null)
            {
                url += "?StaffSearch=" + StaffSearch;
            }
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<StaffDto> staffs = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;

            return View(staffs);
        }

        // GET: Staff/Details/5
        [Authorize]
        public ActionResult Details(int id)
        { 
            DetailsStaff detailsStaff = new DetailsStaff();
            string url = "StaffData/FindStaff/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StaffDto staff = response.Content.ReadAsAsync <StaffDto>().Result;
            detailsStaff.staff = staff;
            url = "AppointmentData/FindAppointmentsForStaff/" + id;
            response = client.GetAsync(url).Result;
            detailsStaff.appointments = response.Content.ReadAsAsync <IEnumerable<AppointmentDto>>().Result;
            return View(detailsStaff);
        }

        // GET: Staff/New
        public ActionResult New()
        {

            string url = "DepartmentData/ListDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            return View(departments);
        }

        // POST: Staff/Create
        [Authorize]
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
        [Authorize]
        public ActionResult Edit(int id)
        {
            UpdateStaff updateStaff = new UpdateStaff();
            string url = "DepartmentData/ListDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            updateStaff.AvailableDepartments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
           
            url = "StaffData/FindStaff/" + id;
            response = client.GetAsync(url).Result;
            updateStaff.SelectedStaff = response.Content.ReadAsAsync<StaffDto>().Result;

            return View(updateStaff);
        }

        // POST: Staff/Edit/5
        [HttpPost]
        [Authorize]
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
        [Authorize]
        public ActionResult Remove(int id)
        {
            string url = "StaffData/FindStaff/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StaffDto staff = response.Content.ReadAsAsync<StaffDto>().Result;
            return View(staff);
        }

        // POST: Staff/Delete/5
        [HttpPost]
        [Authorize]
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
