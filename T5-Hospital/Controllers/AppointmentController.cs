﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net.Http;
using T5_Hospital.Models;

namespace T5_Hospital.Controllers
{
    public class AppointmentController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static AppointmentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }

        // GET: Appointment
        public ActionResult List()
        {

            string url = "AppointmentData/ListAppointment";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<AppointmentDto> appointments = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;
            return View(appointments);
        }

        // GET: Appointment/Details/5
        public ActionResult Details(int id)
        {
            string url = "AppointmentData/FindAppointment/" +id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AppointmentDto appointments = response.Content.ReadAsAsync<AppointmentDto>().Result;
            return View(appointments);
        }

        // GET: Appointment/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Appointment/Create
        [HttpPost]
        public ActionResult Create(Appointment appointment)
        {
            string url = "AppointmentData/AddAppointment";
            string jsonpayload = jss.Serialize(appointment);
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

        // GET: Appointment/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "AppointmentData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AppointmentDto appointments = response.Content.ReadAsAsync<AppointmentDto>().Result;
            return View(appointments);
        }

        // POST: Appointment/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Appointment appointment)
        {
            string url = "AppointmentData/UpdateAppointment/" + id;

            string jsonpayload = jss.Serialize(appointment);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
               return  RedirectToAction("Details/" + id);
            } else
            {
                return View("Error");
            }

        }

        // GET: Appointment/Remove/5
        public ActionResult Remove(int id)
        {
            string url = "AppointmentData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AppointmentDto appointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
            return View(appointment);
        }

        // POST: Appointment/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "AppointmentData/DeleteAppointment/" + id;

            HttpContent content = new StringContent("");

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if(response.IsSuccessStatusCode)
            {
                // TODO: Add delete logic here

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
