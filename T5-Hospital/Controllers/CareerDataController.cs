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
        public IEnumerable<CareerDto> ListCareers(string careerSearch = null)
        {
            List<Career> CareerList = db.Careers.ToList();
            List<CareerDto> CareerDtoList = new List<CareerDto>();

            if(careerSearch != null)
            {
                CareerList = db.Careers.Where(c => c.Title.Contains(careerSearch)).ToList();
            }
            else
            {
                CareerList = db.Careers.ToList();
            }

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
        /// <param name="id">Career Id.</param>
        /// <returns>Using db, it shows entered career by its id</returns>
        
        // GET: api/CareerData/FindCareer/5
        [ResponseType(typeof(CareerDto))]
        [HttpGet]
        public IHttpActionResult FindCareer(int id)
        {
            Career career = db.Careers.Find(id);
            if (career == null)
            {
                return NotFound();
            }
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
            

            return Ok(careerDto);
        }

        /// <summary>
        ///Gets a list of careers of a particular department
        /// </summary>
        /// <param name="id">Department Id.</param>
        /// <returns>list of careers with department details</returns>

        // GET: api/CareerData/FindCareersForDepartment/5
        [ResponseType(typeof(CareerDto))]
        [HttpGet]
        public IHttpActionResult FindCareersForDepartment(int id)
        {
            List<Career> careers = db.Careers.Where(career => career.DepartmentId == id).ToList();
            List<CareerDto> careerDtos = new List<CareerDto>();

            careers.ForEach(career => careerDtos.Add(new CareerDto()
            {
                JobId = career.JobId,
                JobTitle = career.Title,
                JobDescription = career.Description,
                Experience_In_Years = career.Experience_In_Years,
                DepartmentId = career.Department.DepartmentId,
                DepartmentName = career.Department.Name,
                DepartmentDescription = career.Department.Description
            }));

            return Ok(careerDtos);
        }

        /// <summary>
        /// it takes specific career based on its id (JobId) and edit|update it with the information changed
        /// </summary>
        /// <param name="id">Career Id.</param>
        /// <param name="career">Updated Career details</param>
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
        /// <param name="career">New career detail.</param>
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
        /// <param name="id">Career Id.</param>
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