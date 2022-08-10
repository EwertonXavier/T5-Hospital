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
        public IEnumerable<DonationDto> ListDonations(string DonationSearch = null)
        {
            List<Donation> Donations;
            List<DonationDto> DonationDtoList = new List<DonationDto>();

            if (DonationSearch != null)
                Donations = db.Donations.Where(donation => donation.Donor.DonorFirstName == DonationSearch).ToList();
            else
                Donations = db.Donations.ToList();

            Donations.ForEach(donation => DonationDtoList.Add(new DonationDto()
            {
                DonationId = donation.DonationId,
                DonationDescription = donation.DonationDescription,
                DonationDate = donation.DonationDate,
                DonationAmount = donation.DonationAmount,
                DonorId = donation.Donor.DonorId,
                DonorFirstName = donation.Donor.DonorFirstName,
                DonorLastName = donation.Donor.DonorLastName,
                DepartmentId = donation.Department.DepartmentId,
                DepartmentName = donation.Department.Name
            }));
            return DonationDtoList;
        }


        /// <summary>
        /// Gather information about all donations related to a donor
        /// </summary>
        /// <returns>
        /// All Donations in the databas, including thier associated Donor
        /// </returns>
        /// <param name="id">Donor ID.</param>

        /// GET: api/DonationData/FindDonationsForDonor/3
        [HttpGet]
        [ResponseType(typeof(DonationDto))]
        public IHttpActionResult FindDonationsForDonor(int id)
        {
            List<Donation> Donations = db.Donations.Where(d => d.DonorId == id).ToList();
            List<DonationDto> DonationDtos = new List<DonationDto>();

            Donations.ForEach(d => DonationDtos.Add(new DonationDto()
            {
                DonationId = d.DonationId,
                DonationDescription = d.DonationDescription,
                DonationDate = d.DonationDate,
                DonationAmount = d.DonationAmount,
                DonorId = d.Donor.DonorId,
                DonorFirstName = d.Donor.DonorFirstName,
                DonorLastName = d.Donor.DonorLastName,
                DepartmentId = d.Department.DepartmentId,
                DepartmentName = d.Department.Name
            }));
            return Ok(DonationDtos);
        }

        /// <summary>
        /// Gather information about all donations related to a department
        /// </summary>
        /// <returns>
        /// All Donations in the database, including the associated Department
        /// </returns>
        /// <param name="id">Department ID.</param>

        /// GET: api/DonationData/FindDonationsForDepartment/3
        [HttpGet]
        [ResponseType(typeof(DonationDto))]
        public IHttpActionResult FindDonationsForDepartment(int id)
        {
            //all donation to a department which matches the ID
            List<Donation> Donations = db.Donations.Where(d => d.DepartmentId == id).ToList();
            List<DonationDto> DonationDtos = new List<DonationDto>();

            Donations.ForEach(d => DonationDtos.Add(new DonationDto()
            {
                DonationId = d.DonationId,
                DonationDescription = d.DonationDescription,
                DonationDate = d.DonationDate,
                DonationAmount = d.DonationAmount,
                DonorId = d.Donor.DonorId,
                DonorFirstName = d.Donor.DonorFirstName,
                DonorLastName = d.Donor.DonorLastName,
                DepartmentId = d.Department.DepartmentId,
                DepartmentName = d.Department.Name
            }));
            return Ok(DonationDtos);
        }



        /// <summary>
        /// Gets a specific donation by its id and display it
        /// </summary>
        /// <param name="id">donation id</param>
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
                DonorLastName = Donation.Donor.DonorLastName,
                DepartmentId = Donation.Department.DepartmentId,
                DepartmentName = Donation.Department.Name
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
        /// <param name="id">donation id</param>
        /// <param name="donation">donation detail</param>
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
        /// <param name="donation">donation detail</param>
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
        /// <param name="id">donation id</param>
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