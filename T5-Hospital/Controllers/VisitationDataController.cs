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
    public class VisitationDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves all of the visitation records in the database
        /// </summary>
        /// <example>GET api/VisitationData/ListVisitations</example>
        /// <returns>List of VisitationDto objects</returns>
        [HttpGet]
        public IEnumerable<VisitationDto> ListVisitations()
        {
            List<Visitation> Visitations = db.Visitations.ToList();
            List<VisitationDto> VisitationDtos = new List<VisitationDto>();

            Visitations.ForEach(visitation => VisitationDtos.Add(new VisitationDto()
            {
                VisitationId = visitation.VisitationId,
                ArrivalDate = visitation.ArrivalDate,
                DepartureDate = visitation.DepartureDate,
                RelationToVisitor = visitation.RelationToVisitor,
                PatientId = visitation.Patient.PatientId,
                PatientFirstName = visitation.Patient.FirstName,
                VisitorId = visitation.Visitor.VisitorId,
                VisitorFirstName = visitation.Visitor.FirstName
            }));

            return VisitationDtos;
        }

        /// <summary>
        /// Retrieves the visitation records specified by the passed in id
        /// </summary>
        /// <param name="id">Integer ID of the visitation record to be retrieved</param>
        /// <example>GET api/VisitationData/FindVisitation/5</example>
        /// <returns>VisitationDto object</returns>
        [HttpGet]
        [ResponseType(typeof(VisitationDto))]
        public IHttpActionResult FindVisitation(int id)
        {
            Visitation Visitation = db.Visitations.Find(id);
            VisitationDto VisitationDto = new VisitationDto()
            {
                VisitationId = Visitation.VisitationId,
                ArrivalDate = Visitation.ArrivalDate,
                DepartureDate = Visitation.DepartureDate,
                RelationToVisitor = Visitation.RelationToVisitor,
                PatientId = Visitation.Patient.PatientId,
                PatientFirstName = Visitation.Patient.FirstName,
                VisitorId = Visitation.Visitor.VisitorId,
                VisitorFirstName = Visitation.Visitor.FirstName
            };

            if (Visitation == null)
            {
                return NotFound();
            }

            return Ok(Visitation);
        }

        /// <summary>
        /// Changes the visitation record data, specified by the id, with the passed in visitation data
        /// </summary>
        /// <param name="id">Integer ID of the visitation record to be altered</param>
        /// <param name="visitation">Visitation data to replace the database record</param>
        /// <example>UPDATE api/VisitationData/UpdateVisitation/5</example>
        /// <returns>Empty response on success</returns>
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateVisitation(int id, Visitation visitation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != visitation.VisitationId)
            {
                return BadRequest();
            }

            db.Entry(visitation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisitationExists(id))
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
        /// Creates a new visitation record in the database using the passed in visitation data
        /// </summary>
        /// <param name="visitation">Visitation object to be added to the database</param>
        /// <example>POST api/VisitationData/AddVisitation</example>
        /// <returns>Visitation object that was added to the database</returns>
        [HttpPost]
        [ResponseType(typeof(Visitation))]
        public IHttpActionResult AddVisitation(Visitation visitation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Visitations.Add(visitation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = visitation.VisitationId }, visitation);
        }

        /// <summary>
        /// Removes a visitation record as specified by the passed in id
        /// </summary>
        /// <param name="id">Integer ID of the visitation record to be removed</param>
        /// <example>DELETE api/VisitationData/5</example>
        /// <returns>Visitation object that was removed from the database</returns>
        [HttpPost]
        [ResponseType(typeof(Visitation))]
        public IHttpActionResult DeleteVisitation(int id)
        {
            Visitation visitation = db.Visitations.Find(id);
            if (visitation == null)
            {
                return NotFound();
            }

            db.Visitations.Remove(visitation);
            db.SaveChanges();

            return Ok(visitation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VisitationExists(int id)
        {
            return db.Visitations.Count(e => e.VisitationId == id) > 0;
        }
    }
}