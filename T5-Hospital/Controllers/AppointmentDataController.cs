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
    public class AppointmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// GET: api/AppointmentData/ListAppointment
        /// Access the appointment table on the database to retrieve all appointments
        /// Return a list of all appointments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<AppointmentDto> ListAppointment()
        {
            List<Appointment> Appointments = db.Appointments.ToList();
            List<AppointmentDto> AppointmentsDto = new List<AppointmentDto>();

            Appointments.ForEach(appointment => AppointmentsDto.Add(new AppointmentDto()
            {
                Id = appointment.Id,
                AppointmentDate = appointment.AppointmentDate,
                PatientId = appointment.Patient.PatientId,
                PatientFirstName = appointment.Patient.FirstName,
                PatientLastName = appointment.Patient.LastName,
                PatientDateOfBirth = appointment.Patient.DateOfBirth,
                StaffId = appointment.Staff.Id,
                StaffName = appointment.Staff.Name,
                StaffDepartmentId = appointment.Staff.DepartmentId,
                StaffDepartmentName = appointment.Staff.Department.Name
            }));
            return AppointmentsDto;
        }
        /// <summary>
        /// GET: api/AppointmentData/FindAppointment/5
        /// Access the appointment table on the database to retrieve a specific appointments
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Data transfer object of the found appointment</returns>

        [ResponseType(typeof(AppointmentDto))]
        [HttpGet]
        public IHttpActionResult FindAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            AppointmentDto appointmentDto = new AppointmentDto()
            {
                Id = appointment.Id,
                AppointmentDate = appointment.AppointmentDate,
                PatientId = appointment.Patient.PatientId,
                PatientFirstName = appointment.Patient.FirstName,
                PatientLastName = appointment.Patient.LastName,
                PatientDateOfBirth = appointment.Patient.DateOfBirth,
                StaffId = appointment.Staff.Id,
                StaffName = appointment.Staff.Name,
                StaffDepartmentId = appointment.Staff.DepartmentId,
                StaffDepartmentName = appointment.Staff.Department.Name
            };
            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointmentDto);
        }
        /// <summary>
        /// POST: api/AppointmentData/UpdateAppointment/5
        /// Update information from an appointment on the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="appointment"></param>
        /// <returns>StatusCode</returns>

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAppointment(int id, Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appointment.Id)
            {
                return BadRequest();
            }

            db.Entry(appointment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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
        /// POST: api/AppointmentData/AddAppointment
        /// Adds a new appointment to the database
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns></returns>
        [ResponseType(typeof(Appointment))]
        [HttpPost]
        public IHttpActionResult AddAppointment(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Appointments.Add(appointment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = appointment.Id }, appointment);
        }

        // 
        /// <summary>
        /// POST: api/AppointmentData/DeleteAppointment/5
        /// Delete an appointment from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Appointment))]
        [HttpPost]
        public IHttpActionResult DeleteAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(appointment);
            db.SaveChanges();

            return Ok(appointment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentExists(int id)
        {
            return db.Appointments.Count(e => e.Id == id) > 0;
        }
    }
}