using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models
{
    public class Donation
    {
        [Key]
        public int DonationId { get; set; }
        public decimal DonationAmount { get; set; }
        public DateTime DonationDate { get; set; }
        public string DonationDescription { get; set; }

        [ForeignKey("Donor")]
        public int DonorId { get; set; }
        public virtual Donor Donor { get; set; }

    }

    public class DonationDto
    {
        public int DonationId { get; set; }
        public decimal DonationAmount { get; set; }
        public DateTime DonationDate { get; set; }
        public string DonationDescription { get; set; }

        public int DonorId { get; set; }
        public string DonorFirstName { get; set; }
        public string DonorLastName { get; set; }
    }
    }