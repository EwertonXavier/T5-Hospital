﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using T5_Hospital.Models;

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

        // GET: Service
        public ActionResult List()
        {
            string url = "ServiceData/ListServices";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ServiceDto> services = response.Content.ReadAsAsync<IEnumerable<ServiceDto>>().Result;
            return View(services);
        }

        // GET: Service/Details/5
        public ActionResult Details(int id)
        {
            string url = "ServiceData/FindService/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ServiceDto service = response.Content.ReadAsAsync<ServiceDto>().Result;
            return View(service);
        }

        // GET: Service/New
        public ActionResult New()
        {
            return View();
        }

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

        // GET: Service/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "ServiceData/FindService/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ServiceDto serviceDto = response.Content.ReadAsAsync<ServiceDto>().Result;
            return View(serviceDto);
        }

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