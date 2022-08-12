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
    public class DepartmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves all departments from Departments table and list them all
        /// </summary>
        /// <returns>it returns a list of departments (DepartmentsDto)</returns>
        // GET: api/DepartmentData/ListDepartments
        [HttpGet]
        public IEnumerable<DepartmentDto> ListDepartments(string departmentSearch = null)
        {
            List<Department> DepartmentList = db.Departments.ToList();
            List<DepartmentDto> DepartmentDtoList = new List<DepartmentDto>();
            
            if(departmentSearch != null)
            {
                DepartmentList = db.Departments.Where(d => d.Name.Contains(departmentSearch)).ToList();
            }
            else
            {
                DepartmentList = db.Departments.ToList();
            }

            DepartmentList.ForEach(department => DepartmentDtoList.Add(new DepartmentDto()
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.Name,
                DepartmentDescription = department.Description
            }));

            return DepartmentDtoList;
        }


        /// <summary>
        /// Gets a specific department by its id and display it
        /// </summary>
        /// <param name="id">Department Id.</param>
        /// <returns>Using database, it shows entered department by its id</returns>
        
        // GET: api/DepartmentData/FindDepartment/5
        [ResponseType(typeof(DepartmentDto))]
        [HttpGet]
        public IHttpActionResult FindDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            DepartmentDto departmentDto = new DepartmentDto()
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.Name,
                DepartmentDescription = department.Description
            };

            return Ok(departmentDto);

        }


        /// <summary>
        /// it takes specific department based on its id (DepartmentId) and edit|update it with the information changed
        /// </summary>
        /// <param name="id">Department Id.</param>
        /// <param name="department">Updated Department Details</param>
        /// <returns>gives users an access to edit|update the information on a News chosen</returns>
        /// 
        // GET: api/DepartmentData/UpdateDepartment/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDepartment(int id, Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != department.DepartmentId)
            {
                return BadRequest();
            }

            db.Entry(department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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
        /// Add a Department into the database
        /// </summary>
        /// <param name="department">New Department Detail</param>
        /// <returns>it adds a department to the database table as a new data</returns>
        // POST: api/DepartmentData/AddDepartment
        [ResponseType(typeof(Department))]
        [HttpPost]
        public IHttpActionResult AddDepartment(Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Departments.Add(department);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = department.DepartmentId }, department);
        }

        /// <summary>
        /// Delete any department specified with its id number
        /// </summary>
        /// <param name="id">Department Id.</param>
        /// <returns>delete department (id) from db</returns>
        // DELETE: api/DepartmentData/DeleteDepartment/5
        [ResponseType(typeof(Department))]
        [HttpPost]
        public IHttpActionResult DeleteDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(department);
            db.SaveChanges();

            return Ok(department);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DepartmentId == id) > 0;
        }
    }
}