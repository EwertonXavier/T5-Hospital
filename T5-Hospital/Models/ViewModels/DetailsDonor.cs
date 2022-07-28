using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models.ViewModels
{
    public class DetailsDonor
    {
        public DonorDto SelectedDonor { get; set; }
        public IEnumerable<DonationDto> RelatedDonations { get; set; }
    }
}