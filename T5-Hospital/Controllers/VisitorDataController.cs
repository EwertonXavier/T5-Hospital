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
    public class VisitorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of Visitors stored in the database, containing basic information
        /// </summary>
        /// <example>GET api/VisitorData/ListVisitors</example>
        /// <returns>List of VisitorDto objects</returns>
        [HttpGet]
        public IEnumerable<VisitorDto> ListVisitors(string VisitorSearch = null)
        {
            List<Visitor> Visitors;
            List<VisitorDto> VisitorDtos = new List<VisitorDto>();

            if (VisitorSearch != null)
                 Visitors = db.Visitors.Where(visitor => visitor.FirstName == VisitorSearch).ToList();
            else
                 Visitors = db.Visitors.ToList();

            Visitors.ForEach(visitor => VisitorDtos.Add(new VisitorDto()
            {
                VisitorId = visitor.VisitorId,
                FirstName = visitor.FirstName,
                LastName = visitor.LastName,
            }));

            return VisitorDtos;
        }

        /// <summary>
        /// Retrieves the data for a visitor specified by the passed in id
        /// </summary>
        /// <param name="id">Integer ID of the visitor to be retrieved</param>
        /// <example>GET api/VisitorData/FindVisitor/5</example>
        /// <returns>VisitorDto object</returns>
        [HttpGet]
        [ResponseType(typeof(VisitorDto))]
        public IHttpActionResult FindVisitor(int id)
        {
            Visitor Visitor = db.Visitors.Find(id);
            VisitorDto VisitorDto = new VisitorDto()
            {
                VisitorId = Visitor.VisitorId,
                FirstName = Visitor.FirstName,
                LastName = Visitor.LastName
            };

            if (Visitor == null)
            {
                return NotFound();
            }

            return Ok(Visitor);
        }

        /// <summary>
        /// Replaces the visitor data in the database, specified by the passed in id, with the additional passed in visitor data
        /// </summary>
        /// <param name="id">Integer ID of visitor to be updated</param>
        /// <param name="visitor">Visitor data to replace the visitor data stored at id</param>
        /// <example>POST api/VisitorData/UpdateVisitor/5</example>
        /// <returns>Empty response on success</returns>
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateVisitor(int id, Visitor visitor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != visitor.VisitorId)
            {
                return BadRequest();
            }

            db.Entry(visitor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisitorExists(id))
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
        /// Creates a new visitor in the database with the passed in visitor data
        /// </summary>
        /// <param name="visitor">Visitor object to be added to the database</param>
        /// <returns>Newly added Visitor object</returns>
        // POST: api/VisitorData/AddVisitor
        [HttpPost]
        [ResponseType(typeof(Visitor))]
        public IHttpActionResult AddVisitor(Visitor visitor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Visitors.Add(visitor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = visitor.VisitorId }, visitor);
        }

        /// <summary>
        /// Removes a visitor from the database as specified by the passed in id
        /// </summary>
        /// <param name="id">Integer ID of the visitor to be removed from the database</param>
        /// <returns>Visitor object that was removed from the database</returns>
        // DELETE: api/VisitorData/5
        [HttpPost]
        [ResponseType(typeof(Visitor))]
        public IHttpActionResult DeleteVisitor(int id)
        {
            Visitor visitor = db.Visitors.Find(id);
            if (visitor == null)
            {
                return NotFound();
            }

            db.Visitors.Remove(visitor);
            db.SaveChanges();

            return Ok(visitor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VisitorExists(int id)
        {
            return db.Visitors.Count(e => e.VisitorId == id) > 0;
        }
    }
}