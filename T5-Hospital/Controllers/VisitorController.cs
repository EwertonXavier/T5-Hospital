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
    public class VisitorController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static VisitorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }

        // GET: Visitor/List
        [Authorize]
        public ActionResult List(string VisitorSearch = null)
        {
            string url = "VisitorData/ListVisitors";

            if (VisitorSearch != null)
                url += "?VisitorSearch=" + VisitorSearch;

            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<VisitorDto> visitors = response.Content.ReadAsAsync<IEnumerable<VisitorDto>>().Result;

            return View(visitors);
        }

        // GET: Visitor/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            // Create an instance of the DetailsVisitor ViewModel
            DetailsVisitor vm = new DetailsVisitor();

            // Retrieve specific visitor info
            string url = "VisitorData/FindVisitor/" + id;
            // Send request
            HttpResponseMessage response = client.GetAsync(url).Result;
            // Store retrieve visitor info in ViewModel
            vm.SelectedVisitor = response.Content.ReadAsAsync<VisitorDto>().Result;

            // Retrieve all of the Patients the visitor is visiting
            url = "VisitationData/FindVisitationsByVisitor/" + id;
            // Send request
            response = client.GetAsync(url).Result;
            // Store retrieve patients info in ViewModel
            vm.PatientsVisiting = response.Content.ReadAsAsync<IEnumerable<VisitationDto>>().Result;

            // Send ViewModel to View
            return View(vm);
        }

        // GET: Visitor/New
        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        // POST: Visitor/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Visitor visitor)
        {
            string url = "VisitorData/AddVisitor";

            string jsonpayload = jss.Serialize(visitor);

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

        // GET: Visitor/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            string url = "VisitorData/FindVisitor/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            VisitorDto visitorDto = response.Content.ReadAsAsync<VisitorDto>().Result;

            return View(visitorDto);
        }

        // POST: Visitor/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, Visitor visitor)
        {
            string url = "VisitorData/UpdateVisitor/" + id;

            string jsonpayload = jss.Serialize(visitor);

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

        // GET: Visitor/Remove/5
        [Authorize]
        public ActionResult Remove(int id)
        {
            string url = "VisitorData/FindVisitor/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            VisitorDto visitorDto = response.Content.ReadAsAsync<VisitorDto>().Result;

            return View(visitorDto);
        }

        // POST: Visitor/Delete/5
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "VisitorData/DeleteVisitor/" + id;

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
