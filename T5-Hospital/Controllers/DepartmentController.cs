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
    public class DepartmentController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DepartmentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }

        // GET: Department
        [Authorize]
        public ActionResult List(string DepartmentSearch)
        {
            string url = "DepartmentData/ListDepartments";

            if (DepartmentSearch != null)
                url += $"?DepartmentSearch={DepartmentSearch}";

            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            return View(departments);
        }

        // GET: Department/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            DetailsDepartmentServices detailsDepartmentServices = new DetailsDepartmentServices();

            string url = "ServiceData/FindServicesByDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            detailsDepartmentServices.Services = response.Content.ReadAsAsync<IEnumerable<ServiceDto>>().Result;

            url = "StaffData/FindStaffsForDepartment/" + id;
            response = client.GetAsync(url).Result;
            detailsDepartmentServices.Staffs = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;

            url = "NewsData/FindNewsByDepartment/" + id;
            response = client.GetAsync(url).Result;
            detailsDepartmentServices.News = response.Content.ReadAsAsync<IEnumerable<NewsDto>>().Result;

            url = "DonationData/FindDonationsForDepartment/" + id;
            response = client.GetAsync(url).Result;
            detailsDepartmentServices.Donations = response.Content.ReadAsAsync<IEnumerable<DonationDto>>().Result;

            url = "CareerData/FindCareersForDepartment/" + id;
            response = client.GetAsync(url).Result;
            detailsDepartmentServices.Careers = response.Content.ReadAsAsync<IEnumerable<CareerDto>>().Result;

            url = "DepartmentData/FindDepartment/" + id;
            response = client.GetAsync(url).Result;
            detailsDepartmentServices.Department = response.Content.ReadAsAsync<DepartmentDto>().Result;

            return View(detailsDepartmentServices);
        }

        // GET: Department/New
        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        // POST: Department/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Department department)
        {
            string url = "DepartmentData/AddDepartment";


            string jsonpayload = jss.Serialize(department);

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

        // GET: Department/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            string url = "DepartmentData/FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DepartmentDto departmentDto = response.Content.ReadAsAsync<DepartmentDto>().Result;
            return View(departmentDto);
        }

        // POST: Department/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, Department department)
        {
            string url = "DepartmentData/UpdateDepartment/" + id;


            string jsonpayload = jss.Serialize(department);

            HttpContent content = new StringContent(jsonpayload);
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


        // POST: Department/Delete/5
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "DepartmentData/DeleteDepartment/" + id;
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
            
        }

        [Authorize]
        public ActionResult Remove(int id)
        {
            string url = "DepartmentData/FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DepartmentDto departmentDto = response.Content.ReadAsAsync<DepartmentDto>().Result;
            return View(departmentDto);
        }

        [Authorize]
        public ActionResult Error()
        {
            return View();
        }
    }
}
