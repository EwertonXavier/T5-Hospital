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
    public class CareerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves all careers from Careers table and list them all
        /// </summary>
        /// <returns>it returns a list of careers (CareerDto)</returns>
        // GET: api/CareerData/ListCareers
        [HttpGet]
        public IEnumerable<CareerDto> ListCareers()
        {
            List<Career> CareerList = db.Careers.ToList();
            List<CareerDto> CareerDtoList = new List<CareerDto>();
            CareerList.ForEach(career => CareerDtoList.Add(new CareerDto()
            {
                JobId = career.JobId,
                JobTitle = career.Title,
                JobDescription = career.Description,
                Experience_In_Years = career.Experience_In_Years,
                DepartmentId = career.Department.DepartmentId,
                DepartmentName = career.Department.Name,
                DepartmentDescription = career.Department.Description
            }));

            return CareerDtoList;
        }

        /// <summary>
        /// Gets a specific career by its id and display it
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Using db, it shows entered career by its id</returns>
        // GET: api/CareerData/FindCareer/5
        [ResponseType(typeof(CareerDto))]
        [HttpGet]
        public IHttpActionResult FindCareer(int id)
        {
            Career career = db.Careers.Find(id);
            CareerDto careerDto = new CareerDto()
            {
                JobId = career.JobId,
                JobTitle = career.Title,
                JobDescription = career.Description,
                Experience_In_Years = career.Experience_In_Years,
                DepartmentId = career.Department.DepartmentId,
                DepartmentName = career.Department.Name,
                DepartmentDescription = career.Department.Description
            };
            if (career == null)
            {
                return NotFound();
            }

            return Ok(careerDto);
        }

        /// <summary>
        /// it takes specific career based on its id (JobId) and edit|update it with the information changed
        /// </summary>
        /// <param name="id"></param>
        /// <param name="career"></param>
        /// <returns>gives users an access to edit|update the information on a career chosen</returns>
        // POST: api/CareerData/UpdateCareer/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCareer(int id, Career career)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != career.JobId)
            {
                return BadRequest();
            }

            db.Entry(career).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CareerExists(id))
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
        /// Add a Career into the database
        /// </summary>
        /// <param name="career"></param>
        /// <returns>it adds a career to the database table as a new data</returns>
        // POST: api/CareerData/AddCareer
        [ResponseType(typeof(Career))]
        [HttpPost]
        public IHttpActionResult AddCareer(Career career)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Careers.Add(career);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = career.JobId }, career);
        }

        /// <summary>
        /// Delete any Career specified with its id number
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Delete career from db</returns>
        // DELETE: api/CareerData/DeleteCareer/5
        [ResponseType(typeof(Career))]
        [HttpPost]
        public IHttpActionResult DeleteCareer(int id)
        {
            Career career = db.Careers.Find(id);
            if (career == null)
            {
                return NotFound();
            }

            db.Careers.Remove(career);
            db.SaveChanges();

            return Ok(career);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CareerExists(int id)
        {
            return db.Careers.Count(e => e.JobId == id) > 0;
        }
    }
}