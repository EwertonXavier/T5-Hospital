using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
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
        public IEnumerable<VisitationDto> ListVisitations(string VisitationSearch = null)
        {
            List<Visitation> Visitations;
            List<VisitationDto> VisitationDtos = new List<VisitationDto>();

            // If user has searched for a visitaiton record, check both visitor and patient names
            if (VisitationSearch != null)
                Visitations = db.Visitations.Where(visitation => visitation.Visitor.FirstName == VisitationSearch || visitation.Patient.FirstName == VisitationSearch).ToList();
            else
                Visitations = db.Visitations.ToList();

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

            return Ok(VisitationDto);
        }

        /// <summary>
        /// Searches for all of the Visitation Records that belong to the Patient specified by the passed in unique id
        /// </summary>
        /// <param name="id">Integer id of the Patient to search for in the Visitation Records</param>
        /// <returns>List of Visitation Records that belong to a Patient</returns>
        [HttpGet]
        [ResponseType(typeof(VisitationDto))]
        public IHttpActionResult FindVisitationsByPatient(int id)
        {
            // Search visitation records that match the same patient id as queried id
            List<Visitation> Visitations = db.Visitations.Where(record => record.PatientId == id).ToList();
            List<VisitationDto> VisitationDtos = new List<VisitationDto>();

            Visitations.ForEach(v => VisitationDtos.Add(new VisitationDto()
            {
                VisitationId = v.VisitationId,
                ArrivalDate = v.ArrivalDate,
                DepartureDate = v.DepartureDate,
                RelationToVisitor = v.RelationToVisitor,
                PatientId = v.Patient.PatientId,
                PatientFirstName = v.Patient.FirstName,
                VisitorId = v.Visitor.VisitorId,
                VisitorFirstName = v.Visitor.FirstName
            }));

            if (Visitations == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(VisitationDtos);
            }
        }

        /// <summary>
        /// Searches for all of the Visitation Records that belong to the Visitor specified by the passed in unique id
        /// </summary>
        /// <param name="id">Integer id of the Visitor to search for in the Visitation Records</param>
        /// <returns>List of Visitation Records that belong to a Patient</returns>
        [HttpGet]
        [ResponseType(typeof(VisitationDto))]
        public IHttpActionResult FindVisitationsByVisitor(int id)
        {
            // Search visitation records that match the same visitor id as queried id
            List<Visitation> Visitations = db.Visitations.Where(record => record.VisitorId == id).ToList();
            List<VisitationDto> VisitationDtos = new List<VisitationDto>();

            Visitations.ForEach(v => VisitationDtos.Add(new VisitationDto()
            {
                VisitationId = v.VisitationId,
                ArrivalDate = v.ArrivalDate,
                DepartureDate = v.DepartureDate,
                RelationToVisitor = v.RelationToVisitor,
                PatientId = v.Patient.PatientId,
                PatientFirstName = v.Patient.FirstName,
                VisitorId = v.Visitor.VisitorId,
                VisitorFirstName = v.Visitor.FirstName
            }));

            if (Visitations == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(VisitationDtos);
            }
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