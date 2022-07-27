using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using T5_Hospital.Models;

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
        public ActionResult List()
        {
            string url = "VisitorData/ListVisitors";

            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<VisitorDto> visitors = response.Content.ReadAsAsync<IEnumerable<VisitorDto>>().Result;

            return View(visitors);
        }

        // GET: Visitor/Details/5
        public ActionResult Details(int id)
        {
            string url = "VisitorData/FindVisitor/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            VisitorDto visitor = response.Content.ReadAsAsync<VisitorDto>().Result;

            return View(visitor);
        }

        // GET: Visitor/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Visitor/Create
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
        public ActionResult Edit(int id)
        {
            string url = "VisitorData/FindVisitor/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            VisitorDto visitorDto = response.Content.ReadAsAsync<VisitorDto>().Result;

            return View(visitorDto);
        }

        // POST: Visitor/Edit/5
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
        public ActionResult Remove(int id)
        {
            string url = "VisitorData/FindVisitor/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            VisitorDto visitorDto = response.Content.ReadAsAsync<VisitorDto>().Result;

            return View(visitorDto);
        }

        // POST: Visitor/Delete/5
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
        public ActionResult Error()
        {
            return View();
        }
    }
}
