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
using T5_Hospital.Migrations;
using T5_Hospital.Models;

namespace T5_Hospital.Controllers
{
    public class DonorDataController : ApiController
    {

        //This controller  will retrive the donor information form db.
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrives all donor information from donor table and list them all
        /// </summary>
        /// <returns>It returns a list of donors (DonorDto)</returns>
        // GET: api/DonorData/ListDonors
        [HttpGet]
        public IEnumerable<DonorDto> ListDonors(string DonorSearch = null)
        {
            List<Donor> Donors;
            List<DonorDto> DonorDtoList = new List<DonorDto>();

            if (DonorSearch != null)
                Donors = db.Donors.Where(donor => donor.DonorFirstName == DonorSearch).ToList();
            else
                Donors = db.Donors.ToList();

            Donors.ForEach(donor => DonorDtoList.Add(new DonorDto()
            {
                DonorId = donor.DonorId,
                DonorFirstName = donor.DonorFirstName,
                DonorLastName = donor.DonorLastName,
                DonorEmail = donor.DonorEmail,
                DonorPhone = donor.DonorPhone
            }));
            return DonorDtoList;
        }


        /// <summary>
        /// Gets a specific donor by its id and display it
        /// </summary>
        /// <returns>Using database, it shows entered donor by its id</returns>
        // GET: api/DonorData/FindDonor/5
        [HttpGet]
        [ResponseType(typeof(DonorDto))]
        public IHttpActionResult FindDonor(int id)
        {
            Donor Donor = db.Donors.Find(id);
            DonorDto DonorDto = new DonorDto()
            {
                DonorId = Donor.DonorId,
                DonorFirstName = Donor.DonorFirstName,
                DonorLastName = Donor.DonorLastName,
                DonorEmail = Donor.DonorEmail,
                DonorPhone = Donor.DonorPhone,
            };
            if (Donor == null)
            {
                return NotFound();
            }
            return Ok(DonorDto);
        }

        /// <summary>
        /// it takes specific donor based on its id (DonorId) and edit|update it with the information changed
        /// </summary>
        /// <param name="id">donor id</param>
        /// <param name="donor">donor detail</param>
        /// <returns>Gives users an access to edit|update the information on a Donor chosen</returns>
        // POst: api/DonorData/UpdateDonor/5
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateDonor(int id, Donor donor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != donor.DonorId)
            {
                return BadRequest();
            }
            db.Entry(donor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonorExists(id))
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
        /// Add a donor into the database
        /// </summary>
        /// <param name="donor">donor detail</param>
        /// <returns>It adds a donor to the database table as a new data</returns>
        // POST: api/DonorData/AddDonor
        [HttpPost]
        [ResponseType(typeof(Donor))]
        public IHttpActionResult AddDonor(Donor donor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Donors.Add(donor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = donor.DonorId }, donor);
        }


        /// <summary>
        /// Delete any donor specified with its id number
        /// </summary>
        /// <param name="id">donor id</param>
        /// <returns>delete donor (id) from db</returns>
        // POST: api/DonorData/DeleteDonor/5
        [HttpPost]
        [ResponseType(typeof(Donor))]
        public IHttpActionResult DeleteDonor(int id)
        {
            Donor donor = db.Donors.Find(id);
            if (donor == null)
            {
                return NotFound();
            }

            db.Donors.Remove(donor);
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

        private bool DonorExists(int id)
        {
            return db.Donors.Count(e => e.DonorId == id) > 0;
        }
    }
}