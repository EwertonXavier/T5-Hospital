using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models.ViewModels
{
    public class UpdateDonation
    {
        public DonationDto SelectedDonation { get; set; }

        //list of Donors
        public IEnumerable<DonorDto> DonorOptions { get; set; }
    }
}