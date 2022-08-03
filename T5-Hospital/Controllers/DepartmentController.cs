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
        public ActionResult List()
        {
            string url = "DepartmentData/ListDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            return View(departments);
        }

        // GET: Department/Details/5
        public ActionResult Details(int id)
        {
            DetailsDepartmentServices detailsDepartment = new DetailsDepartmentServices();
            string url = "DepartmentData/FindServicesByDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            detailsDepartment.Services = response.Content.ReadAsAsync<IEnumerable<ServiceDto>>().Result;
            url = "DepartmentData/FindDepartment/" + id;
            response = client.GetAsync(url).Result;
            detailsDepartment.Department = response.Content.ReadAsAsync<DepartmentDto>().Result;
            return View(detailsDepartment);
        }

        // GET: Department/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Department/Create
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
        public ActionResult Edit(int id)
        {
            string url = "DepartmentData/FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DepartmentDto departmentDto = response.Content.ReadAsAsync<DepartmentDto>().Result;
            return View(departmentDto);
        }

        // POST: Department/Edit/5
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


        public ActionResult Remove(int id)
        {
            string url = "DepartmentData/FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DepartmentDto departmentDto = response.Content.ReadAsAsync<DepartmentDto>().Result;
            return View(departmentDto);
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
