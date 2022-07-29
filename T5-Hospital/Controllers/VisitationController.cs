﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using T5_Hospital.Models;
using T5_Hospital.Models.ViewModels;

namespace T5_Hospital.Controllers
{
    public class VisitationController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static VisitationController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }

        // GET: Visitation/List
        public ActionResult List()
        {
            string url = "VisitationData/ListVisitations";

            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<VisitationDto> visitationRecords = response.Content.ReadAsAsync<IEnumerable<VisitationDto>>().Result;

            return View(visitationRecords);
        }

        // GET: Visitation/Details/5
        public ActionResult Details(int id)
        {
            string url = "VisitationData/FindVisitation/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            VisitationDto visitationRecord = response.Content.ReadAsAsync<VisitationDto>().Result;

            return View(visitationRecord);
        }

        // GET: Visitation/New
        public ActionResult New()
        {
            NewVisitation vm = new NewVisitation();

            // Make request to retrieve all current patients
            string url = "PatientData/ListPatients";
            // Send request
            HttpResponseMessage response = client.GetAsync(url).Result;
            // Store received patients in ViewModel
            vm.CurrentPatients = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;

            // Make request to retrieve all current visitors
            url = "VisitorData/ListVisitors";
            // Send request
            response = client.GetAsync(url).Result;
            // Store received visitors in ViewModel
            vm.CurrentVisitors = response.Content.ReadAsAsync<IEnumerable<VisitorDto>>().Result;

            return View(vm);
        }

        // POST: Visitation/Create
        [HttpPost]
        public ActionResult Create(Visitation visitation)
        {
            string url = "VisitationData/AddVisitation";

            string jsonpayload = jss.Serialize(visitation);

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

        // GET: Visitation/Edit/5
        public ActionResult Edit(int id)
        {
            // Create a new UpdateVisitation ViewModel to store the current Visitation Record to be added
            //  and all of the Current Visitors and Patients
            UpdateVisitation vm = new UpdateVisitation();

            // Make a request to get the specified visitation record data
            string url = "VisitationData/FindVisitation/" + id;
            // Set request
            HttpResponseMessage response = client.GetAsync(url).Result;
            // Store visitation record data in ViewModel
            vm.VisitationRecord = response.Content.ReadAsAsync<VisitationDto>().Result;

            // Make request to retrieve all current patients
            url = "PatientData/ListPatients";
            // Send request
            response = client.GetAsync(url).Result;
            // Store received patients in ViewModel
            vm.CurrentPatients = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;

            // Make request to retrieve all current visitors
            url = "VisitorData/ListVisitors";
            // Send request
            response = client.GetAsync(url).Result;
            // Store received visitors in ViewModel
            vm.CurrentVisitors = response.Content.ReadAsAsync<IEnumerable<VisitorDto>>().Result;

            return View(vm);
        }

        // POST: Visitation/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Visitation visitation)
        {
            string url = "VisitationData/UpdateVisitation/" + id;

            string jsonpayload = jss.Serialize(visitation);

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

        // GET: Visitation/Remove/5
        public ActionResult Remove(int id)
        {
            Debug.WriteLine(id);
            string url = "VisitationData/FindVisitation/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            VisitationDto visitationDto = response.Content.ReadAsAsync<VisitationDto>().Result;

            return View(visitationDto);
        }

        // POST: Visitation/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "VisitationData/DeleteVisitation/" + id;

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
