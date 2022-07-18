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
    public class DonationDataController : ApiController
    {
        //This controller  will retrive the donation information form db.
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrives all donation information from donation table and list them all
        /// </summary>
        /// <returns>It returns a list of donation (DonationDto)</returns>
        // GET: api/DonationData/ListDonations
        [HttpGet]
        public IEnumerable<DonationDto> ListDonations()
        {
            List<Donation> DonationList = db.Donations.ToList();
            List<DonationDto> DonationDtoList = new List<DonationDto>();

            DonationList.ForEach(donation => DonationDtoList.Add(new DonationDto()
            {
                DonationId = donation.DonationId,
                DonationDescription = donation.DonationDescription,
                DonationDate = donation.DonationDate,
                DonationAmount = donation.DonationAmount,
                DonorId = donation.Donor.DonorId,
                DonorFirstName = donation.Donor.DonorFirstName,
                DonorLastName = donation.Donor.DonorLastName
            }));
            return DonationDtoList;
        }

        /// <summary>
        /// Gets a specific donation by its id and display it
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Using database, it shows entered donation by its id</returns>
        // GET: api/DonationData/FindDonation/5
        [HttpGet]
        [ResponseType(typeof(DonationDto))]
        public IHttpActionResult FindDonation(int id)
        {
            Donation Donation = db.Donations.Find(id);
            DonationDto DonationDto = new DonationDto()
            {
                DonationId = Donation.DonationId,
                DonationDescription = Donation.DonationDescription,
                DonationDate = Donation.DonationDate,
                DonationAmount = Donation.DonationAmount,
                DonorId = Donation.Donor.DonorId,
                DonorFirstName = Donation.Donor.DonorFirstName,
                DonorLastName = Donation.Donor.DonorLastName
            };
            if (Donation == null)
            {
                return NotFound();
            }

            return Ok(DonationDto);
        }


        /// <summary>
        /// it takes specific donation based on its id (DonationId) and edit|update it with the information changed
        /// </summary>
        /// <param name="id"></param>
        /// <param name="donation"></param>
        /// <returns>Gives users an access to edit|update the information on a Donation chosen</returns>
        // POST: api/DonationData/UpdateDonation/5
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateDonation(int id, Donation donation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != donation.DonationId)
            {
                return BadRequest();
            }

            db.Entry(donation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonationExists(id))
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
        /// Add a donation information into the database
        /// </summary>
        /// <param name="donation"></param>
        /// <returns>It adds a donation to the database table as a new data</returns>
        // POST: api/DonationData/AddDonation
        [HttpPost]
        [ResponseType(typeof(Donation))]
        public IHttpActionResult AddDonation(Donation donation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Donations.Add(donation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = donation.DonationId }, donation);
        }


        /// <summary>
        /// Delete any donation specified with its id number
        /// </summary>
        /// <param name="id"></param>
        /// <returns>delete donation (id) from db</returns>
        // POST: api/DonationData/DeleteDonation/5
        [HttpPost]
        [ResponseType(typeof(Donation))]
        public IHttpActionResult DeleteDonation(int id)
        {
            Donation donation = db.Donations.Find(id);
            if (donation == null)
            {
                return NotFound();
            }

            db.Donations.Remove(donation);
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

        private bool DonationExists(int id)
        {
            return db.Donations.Count(e => e.DonationId == id) > 0;
        }
    }
}