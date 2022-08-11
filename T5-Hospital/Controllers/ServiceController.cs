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
    public class ServiceController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ServiceController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }

        [Authorize]
        // GET: Service
        public ActionResult List(string ServiceSearch = null)
        {
            string url = "ServiceData/ListServices";

            if (ServiceSearch != null)
            {
                url += "?ServiceSearch=" + ServiceSearch;
            }

            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ServiceDto> services = response.Content.ReadAsAsync<IEnumerable<ServiceDto>>().Result;
            return View(services);
        }

        [Authorize]
        // GET: Service/Details/5
        public ActionResult Details(int id)
        {
            string url = "ServiceData/FindService/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ServiceDto service = response.Content.ReadAsAsync<ServiceDto>().Result;
            return View(service);
        }

        [Authorize]
        // GET: Service/New
        public ActionResult New()
        {
            string url = "DepartmentData/ListDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            return View(departments);
        }

        [Authorize]
        // POST: Service/Create
        [HttpPost]
        public ActionResult Create(Service service)
        {
            string url = "ServiceData/AddService";


            string jsonpayload = jss.Serialize(service);

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
        // GET: Service/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateService vm = new UpdateService();
            string url = "DepartmentData/ListDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            vm.AvailableDepartments= response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            url = "ServiceData/FindService/" + id;
            response = client.GetAsync(url).Result;
            vm.SelectedService = response.Content.ReadAsAsync<ServiceDto>().Result;
            
            return View(vm);
        }

        [Authorize]
        // POST: Service/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Service service)
        {
            string url = "ServiceData/UpdateService/" + id;


            string jsonpayload = jss.Serialize(service);

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
        // POST: Service/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "ServiceData/DeleteService/" + id;
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
        // GET: Service/Remove/5
        public ActionResult Remove(int id)
        {
            string url = "ServiceData/FindService/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ServiceDto serviceDto = response.Content.ReadAsAsync<ServiceDto>().Result;
            return View(serviceDto);
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
