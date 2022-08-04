using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models.ViewModels
{
    public class DetailsDepartmentServices
    {
        public IEnumerable<ServiceDto> Services { get; set; }
        public IEnumerable<StaffDto> Staffs { get; set; }
        public IEnumerable<NewsDto> News { get; set; }
        public IEnumerable<DonationDto> Donations { get; set; }
        public IEnumerable<CareerDto> Careers { get; set; }
        public DepartmentDto Department { get; set; }
    }
}