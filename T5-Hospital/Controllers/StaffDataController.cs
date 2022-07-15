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
    /// <summary>
    /// This datacontroller is going to retrieve data from the database upon request from the view Controllers
    /// </summary>
    public class StaffDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Retrieves information from Staff Table and returns a list of Staff
        /// Route: GET: api/StaffData/ListStaff
        /// </summary>
        /// <returns>
        /// List<StaffDto> 
        /// </returns>
        [HttpGet]
        public IEnumerable<StaffDto> ListStaff()
        {
            //DB.staff is the name of the Dataset we created in the Identity model
            List<Staff> StaffList = db.Staff.ToList();
            List<StaffDto> StaffDtoList = new List<StaffDto>();
            StaffList.ForEach(staff =>
           StaffDtoList.Add(new StaffDto() {
               Id = staff.Id,
               Name = staff.Name,
               Email = staff.Email,
               DepartmentId = staff.Department.DepartmentId,
               DepartmentName = staff.Department.Name,
               DepartmentDescription = staff.Department.Description

           }));
              
            return StaffDtoList;
        }

        /// <summary>
        /// Retrieves information from Staff Table and returns a specific Staff member
        /// Route: GET: api/StaffsData/FindStaff/5
        /// </summary>
        /// <returns>
        /// StaffDto
        /// </returns>
        [ResponseType(typeof(StaffDto))]
        [HttpGet]
        public IHttpActionResult FindStaff(int id)
        {
            Staff Staff = db.Staff.Find(id);
            StaffDto StaffDto = new StaffDto()
            {
                Id = Staff.Id,
                Name = Staff.Name,
                Email = Staff.Email,
                DepartmentId = Staff.Department.DepartmentId,
                DepartmentName = Staff.Department.Name,
                DepartmentDescription = Staff.Department.Description

            };
            if (Staff == null)
            {
                return NotFound();
            }

            return Ok(StaffDto);
        }


        /// <summary>
        /// Edit a specific Staff member information from Staff Table
        /// Route: POST: api/StaffData/UpdateStaff/5
        /// </summary>
        /// <returns>
        /// Http StatusCode
        /// </returns>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateStaff(int id, Staff staff)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != staff.Id)
            {
                return BadRequest();
            }

            db.Entry(staff).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffExists(id))
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
        /// Add a new Staff member in the Staff Table
        /// Route: POST: api/StaffData/AddStaff
        /// </summary>
        /// <returns>
        /// Http StatusCode
        /// </returns>
        [ResponseType(typeof(Staff))]
        [HttpPost]
        public IHttpActionResult AddStaff(Staff staff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Staff.Add(staff);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = staff.Id }, staff);
        }


        /// <summary>
        /// Remove a specific Staff member from the Staff Table
        /// Route: POST: api/StaffData/DeleteStaff/5
        /// </summary>
        /// <returns>
        /// Http StatusCode
        /// </returns>
        [ResponseType(typeof(Staff))]
        [HttpPost]
        public IHttpActionResult DeleteStaff(int id)
        {
            Staff staff = db.Staff.Find(id);
            if (staff == null)
            {
                return NotFound();
            }

            db.Staff.Remove(staff);
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

        private bool StaffExists(int id)
        {
            return db.Staff.Count(e => e.Id == id) > 0;
        }
    }
}