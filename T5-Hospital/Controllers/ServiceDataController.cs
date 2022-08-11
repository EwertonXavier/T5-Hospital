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
    public class ServiceDataController : ApiController
    {
        //This controller will retrieve Services data from db and let admin(s) to manage CRUD functionality
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves all Services from Services table and list them all
        /// </summary>
        /// <returns>Returns a list of Services</returns>
        // GET: api/ServiceData/ListServices
        [HttpGet]
        public IEnumerable<ServiceDto> ListServices(string ServiceSearch = null)
        {
            List<Service> ServiceList = db.Services.ToList();
            List<ServiceDto> ServiceDtoList = new List<ServiceDto>();

            if (ServiceSearch != null)
                ServiceList = db.Services.Where(Service => Service.ServiceName == ServiceSearch).ToList();
            else
                ServiceList = db.Services.ToList();

            ServiceList.ForEach(service =>
           ServiceDtoList.Add(new ServiceDto()
           {
               ServiceId = service.ServiceId,
               ServiceName = service.ServiceName,
               ServiceDetail = service.ServiceDetail,
               DepartmentId = service.Department.DepartmentId,
               DepartmentName = service.Department.Name,
               DepartmentDescription = service.Department.Description

           }));
            return ServiceDtoList;
        }

        /// <summary>
        /// Gets a specific service by its id and display it
        /// </summary>
        /// <param name="id">takes ServiceId</param>
        /// <returns>Using db, it shows entered service by its id</returns>
        // GET: api/ServiceData/FindService/5
        [ResponseType(typeof(Service))]
        [HttpGet]
        public IHttpActionResult FindService(int id)
        {
            Service Service = db.Services.Find(id);
            ServiceDto ServiceDto = new ServiceDto()
            {
                ServiceId = Service.ServiceId,
                ServiceName = Service.ServiceName,
                ServiceDetail = Service.ServiceDetail,
                DepartmentId = Service.Department.DepartmentId,
                DepartmentName = Service.Department.Name,
                DepartmentDescription = Service.Department.Description
            };

            if (Service == null)
            {
                return NotFound();
            }

            return Ok(ServiceDto);
        }


        /// <summary>
        /// takes all services associated with the department specified by its id
        /// </summary>
        /// <param name="id">takes DepartmentId</param>
        /// <returns></returns>
        [ResponseType(typeof(ServiceDto))]
        [HttpGet]
        public IHttpActionResult FindServicesByDepartment(int id)
        {
            List<Service> services = db.Services.Where(s => s.DepartmentId == id).ToList();
            List<ServiceDto> serviceDtos = new List<ServiceDto>();
            if (services == null)
            {
                return NotFound();
            }

            services.ForEach(service => serviceDtos.Add(new ServiceDto()
            {
                ServiceId = service.ServiceId,
                ServiceName = service.ServiceName,
                DepartmentId = service.Department.DepartmentId,
                DepartmentName = service.Department.Name,
                DepartmentDescription = service.Department.Description

            }));

            return Ok(serviceDtos);
        }

        /// <summary>
        /// it takes specific service based on its id (ServiceId) and edit|update it with the information changed
        /// </summary>
        /// <param name="id">takes ServiceId</param>
        /// <param name="service">takes Service Model</param>
        /// <returns>gives users an access to edit|update the information on a Service chosen</returns>
        // POST: api/ServiceData/UpdateService/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateService(int id, Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != service.ServiceId)
            {
                return BadRequest();
            }

            db.Entry(service).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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
        /// Add a Service into the database
        /// </summary>
        /// <param name="service">takes Service Model</param>
        /// <returns>it adds a news to the database table as a new data</returns>
        // POST: api/ServiceData/AddService
        [ResponseType(typeof(Service))]
        [HttpPost]
        public IHttpActionResult AddService(Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Services.Add(service);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = service.ServiceId }, service);
        }


        /// <summary>
        /// Delete any Service specified with its id number
        /// </summary>
        /// <param name="id">takes ServiceId</param>
        /// <returns>Delete service from db</returns>
        // DELETE: api/ServiceData/DeleteService/5
        [ResponseType(typeof(Service))]
        [HttpPost]
        public IHttpActionResult DeleteService(int id)
        {
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            db.Services.Remove(service);
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

        private bool ServiceExists(int id)
        {
            return db.Services.Count(e => e.ServiceId == id) > 0;
        }
    }
}