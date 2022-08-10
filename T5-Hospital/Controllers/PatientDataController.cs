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
    public class PatientDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieve a list of PatientDto objects containing the data related to each patient
        /// </summary>
        /// <example>GET api/PatientData/ListPatients</example>
        /// <returns>IEnummerable list of PatientDto objects</returns>
        [HttpGet]
        public IEnumerable<PatientDto> ListPatients(string PatientSearch = null)
        {
            List<Patient> Patients;
            List<PatientDto> PatientDtos = new List<PatientDto>();

            if (PatientSearch != null)
                Patients = db.Patients.Where(patient => patient.FirstName == PatientSearch).ToList();
            else
                Patients = db.Patients.ToList();

            Patients.ForEach(patient => PatientDtos.Add(new PatientDto()
            {
                PatientId = patient.PatientId,
                FirstName=patient.FirstName,
                LastName=patient.LastName,
                DateOfBirth=patient.DateOfBirth,
            }));

            return PatientDtos;
        }

        /// <summary>
        /// Retrieves the details of a patient specified by the passed in id
        /// </summary>
        /// <param name="id">Integer ID of the specified Patient</param>
        /// <example>GET api/PatientData/FindPatient/5</example>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(PatientDto))]
        public IHttpActionResult FindPatient(int id)
        {
            Patient Patient = db.Patients.Find(id);
            PatientDto PatientDto = new PatientDto()
            {
                PatientId = Patient.PatientId,
                FirstName = Patient.FirstName,
                LastName = Patient.LastName,
                DateOfBirth = Patient.DateOfBirth
            };

            if (Patient == null)
            {
                return NotFound();
            }

            return Ok(Patient);
        }

        /// <summary>
        /// Changes the data of the Patient found at the passed in id, with the additional data passed in
        /// </summary>
        /// <param name="id">Integer ID of the Patient to be update</param>
        /// <param name="patient">Patient information to replace that stored at the passed in id</param>
        /// <example>POST api/PatientData/UpdatePatient/5</example>
        /// <returns>Empty result on success</returns>
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdatePatient(int id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patient.PatientId)
            {
                return BadRequest();
            }

            db.Entry(patient).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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
        /// Creates a new Patient object in the database with the passed in data
        /// </summary>
        /// <param name="patient">Patient object containing the data to be added to the database</param>
        /// <example>POST api/PatientData/AddPatient</example>
        /// <returns>Newly added Patient object</returns>
        [HttpPost]
        [ResponseType(typeof(Patient))]
        public IHttpActionResult AddPatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Patients.Add(patient);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = patient.PatientId }, patient);
        }

        /// <summary>
        /// Removes a Patient object from the database specified by the passed in id
        /// </summary>
        /// <param name="id">Integer ID of the patient to be removed from the database</param>
        /// <example>DELETE api/PatientData/5</example>
        /// <returns>Patient object that was removed from the database</returns>
        [HttpPost]
        [ResponseType(typeof(Patient))]
        public IHttpActionResult DeletePatient(int id)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            db.Patients.Remove(patient);
            db.SaveChanges();

            return Ok(patient);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientExists(int id)
        {
            return db.Patients.Count(e => e.PatientId == id) > 0;
        }
    }
}