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
        public ActionResult List()
        {
            string url = "CareerData/ListCareers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CareerDto> careers = response.Content.ReadAsAsync<IEnumerable<CareerDto>>().Result;
            return View(careers);
        }

        // GET: Career/Details/5
        public ActionResult Details(int id)
        {
            string url = "CareerData/FindCareer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CareerDto department = response.Content.ReadAsAsync<CareerDto>().Result;
            return View(department);
        }

        // GET: Career/Create
        public ActionResult New()
        {
            return View();
        }

        // POST: Career/Create
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
        public ActionResult Edit(int id)
        {
            string url = "CareerData/FindCareer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CareerDto careerDto = response.Content.ReadAsAsync<CareerDto>().Result;
            return View(careerDto);
        }

        // POST: Career/Edit/5
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
        public ActionResult Remove(int id)
        {
            string url = "CareerData/FindCareer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CareerDto careerDto = response.Content.ReadAsAsync<CareerDto>().Result;
            return View(careerDto);
        }

        // POST: Career/Delete/5
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

        public ActionResult Error()
        {
            return View();
        }
    }
}
