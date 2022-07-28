using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models
{
	public class Donor
	{
        [Key]
        public int DonorId { get; set; }
        public string DonorFirstName { get; set; }
        public string DonorLastName { get; set; }
        public string DonorEmail { get; set; }
        public string DonorPhone { get; set; }

        public ICollection<Donation> Donations { get; set; }
    }

    public class DonorDto
    {
        public int DonorId { get; set; }
        public string DonorFirstName { get; set; }
        public string DonorLastName { get; set; }
        public string DonorEmail { get; set; }
        public string DonorPhone { get; set; }
    }


}