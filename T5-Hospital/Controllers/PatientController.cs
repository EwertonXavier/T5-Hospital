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
    public class PatientController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PatientController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }

        // GET: Patient
        [Authorize]
        public ActionResult List(string PatientSearch = null)
        {
            string url = "PatientData/ListPatients";

            if (PatientSearch != null)
                url += "?PatientSearch=" + PatientSearch;

            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<PatientDto> patients = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;

            return View(patients);
        }

        // GET: Patient/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            // Create new DetailsPatient ViewModel for storing patient details and appointments the specified patient has
            DetailsPatient vm = new DetailsPatient();

            // Retrieve specified patient info
            string url = "PatientData/FindPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            // Store the resulting patient details in ViewModel
            vm.SelectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;

            // Retrieve list of appointments the current patient has
            url = "AppointmentData/FindAppointmentsForPatient/" + id;
            // Send request
            response = client.GetAsync(url).Result;
            // Store resulting appointments in VieModel
            vm.Appointments = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;

            // Retrieve a list of all of the visitors a patient has
            url = "VisitationData/FindVisitationsByPatient/" + id;
            // Send request
            response = client.GetAsync(url).Result;
            // Store resulting visitation records in ViewModel
            vm.Visitors = response.Content.ReadAsAsync<IEnumerable<VisitationDto>>().Result;

            return View(vm);
        }

        // GET: Patient/New
        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        // POST: Patient/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Patient patient)
        {
            string url = "PatientData/AddPatient";

            string jsonpayload = jss.Serialize(patient);

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

        // GET: Patient/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            string url = "PatientData/FindPatient/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto patientDto = response.Content.ReadAsAsync<PatientDto>().Result;

            return View(patientDto);
        }

        // POST: Patient/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, Patient patient)
        {
            string url = "PatientData/UpdatePatient/" + id;

            string jsonpayload = jss.Serialize(patient);

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

        // GET: Patient/Remove/5
        [Authorize]
        public ActionResult Remove(int id)
        {
            string url = "PatientData/FindPatient/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto patientDto = response.Content.ReadAsAsync<PatientDto>().Result;

            return View(patientDto);
        }

        // POST: Patient/Delete/5
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "PatientData/DeletePatient/" + id;

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
