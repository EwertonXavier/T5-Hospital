﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using T5_Hospital.Models;

namespace T5_Hospital.Controllers
{
    public class NewsController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static NewsController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }
        // GET: News
        public ActionResult List()
        {
            string url = "NewsData/ListNews";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<NewsDto> news = response.Content.ReadAsAsync<IEnumerable<NewsDto>>().Result;
            return View(news);
        }

        // GET: News/Details/5
        public ActionResult Details(int id)
        {
            string url = "NewsData/FindNews/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            NewsDto news = response.Content.ReadAsAsync<NewsDto>().Result;
            return View(news);
        }

        // GET: News/New
        public ActionResult New()
        {
            return View();
        }

        // POST: News/Create
        [HttpPost]
        public ActionResult Create(News news)
        {
            string url = "NewsData/AddNews";


            string jsonpayload = jss.Serialize(news);

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

        // GET: News/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "NewsData/FindNews/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            NewsDto newsDto = response.Content.ReadAsAsync<NewsDto>().Result;
            return View(newsDto);
        }

        // POST: News/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, News news)
        {
            string url = "NewsData/UpdateNews/" + id;


            string jsonpayload = jss.Serialize(news);

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

        // POST: News/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "NewsData/DeleteNews/" + id;
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

        // GET: News/Delete/5
        public ActionResult Remove(int id)
        {
            string url = "NewsData/FindNews/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            NewsDto newsDto = response.Content.ReadAsAsync<NewsDto>().Result;
            return View(newsDto);
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
