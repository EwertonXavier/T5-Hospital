using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using T5_Hospital.Models;

namespace T5_Hospital.Controllers
{
    public class NewsDataController : ApiController
    {
        //This controller will retrieve News data from db and let admin(s) to manage CRUD functionality
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves all News from News table and list them all
        /// </summary>
        /// <returns>it returns a list of news (NewsDto)</returns>
        // GET: api/NewsData/ListNews
        [HttpGet]
        public IEnumerable<NewsDto> ListNews(string NewsSearch = null)
        {
            List<News> NewsList = db.News.ToList();
            List<NewsDto> NewsDtoList = new List<NewsDto>();

            if (NewsSearch != null)
                NewsList = db.News.Where(News => News.NewsTitle == NewsSearch).ToList();
            else
                NewsList = db.News.ToList();

            NewsList.ForEach(news =>
            NewsDtoList.Add(new NewsDto()
            {
                NewsId = news.NewsId,
                NewsTitle = news.NewsTitle,
                NewsContent = news.NewsContent,
                NewsDate = news.NewsDate
            }));

            return NewsDtoList;
        }

        /// <summary>
        /// Gets a specific new by its id and display it
        /// </summary>
        /// <param name="id">takes newsId to display specific news</param>
        /// <returns>Using database, it shows entered news by its id</returns>
        // GET: api/NewsData/FindNews/5
        [ResponseType(typeof(NewsDto))]
        [HttpGet]
        public IHttpActionResult FindNews(int id)
        {
            News news= db.News.Find(id);
            NewsDto NewsDto = new NewsDto()
            {
                NewsId = news.NewsId,
                NewsTitle = news.NewsTitle,
                NewsContent = news.NewsContent,
                NewsDate = news.NewsDate,
                DepartmentId = news.Department.DepartmentId,
                DepartmentName = news.Department.Name,
                DepartmentDescription = news.Department.Description
            };

            if (news == null)
            {
                return NotFound();
            }

            return Ok(NewsDto);

        }


        /// <summary>
        /// Gets all news associated with a department
        /// </summary>
        /// <param name="id">takin DepartmentId</param>
        /// <returns>Returns a list of news associated with a specific department by using departmentid</returns>
        [ResponseType(typeof(NewsDto))]
        [HttpGet]
        public IHttpActionResult FindNewsByDepartment(int id)
        {
            List<News> news = db.News.Where(n => n.DepartmentId == id).ToList();
            List<NewsDto> newsDtos = new List<NewsDto>();
            if (news == null)
            {
                return NotFound();
            }

            news.ForEach(n => newsDtos.Add(new NewsDto()
            {
                NewsId = n.NewsId,
                NewsTitle = n.NewsTitle,
                NewsContent = n.NewsContent,
                NewsDate = n.NewsDate,
                DepartmentId = n.Department.DepartmentId,
                DepartmentName = n.Department.Name,
                DepartmentDescription = n.Department.Description

            }));

            return Ok(newsDtos);
        }

        /// <summary>
        /// it takes specific news based on its id (NewsId) and edit|update it with the information changed
        /// </summary>
        /// <param name="id">takes NewsId</param>
        /// <param name="news">takes News Model</param>
        /// <returns>gives users an access to edit|update the information on a News chosen</returns>
        // GET: api/NewsData/UpdateNews/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateNews(int id, News news)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != news.NewsId)
            {
                return BadRequest();
            }

            db.Entry(news).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Add a News into the database
        /// </summary>
        /// <param name="news">takes News Model</param>
        /// <returns>it adds a news to the database table as a new data</returns>
        // POST: api/NewsData/AddNews
        [ResponseType(typeof(News))]
        [HttpPost]
        public IHttpActionResult AddNews(News news)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.News.Add(news);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = news.NewsId }, news);
        }

        /// <summary>
        /// Delete any news specified with its id number
        /// </summary>
        /// <param name="id">takes NewsId</param>
        /// <returns>delete news (id) from db</returns>
        // DELETE: api/NewsData/DeleteNews/5
        [ResponseType(typeof(News))]
        [HttpPost]
        public IHttpActionResult DeleteNews(int id)
        {
            News news = db.News.Find(id);
            if (news == null)
            {
                return NotFound();
            }

            db.News.Remove(news);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NewsExists(int id)
        {
            return db.News.Count(e => e.NewsId == id) > 0;
        }
    }
}