using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models.ViewModels
{
    public class DetailsDepartmentDonations
    {
        public DepartmentDto Department { get; set; }

        public IEnumerable<DonationDto> DepartmentDonations { get; set; }


    }
}