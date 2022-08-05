using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models.ViewModels
{
    public class DetailsStaff
    {
        public StaffDto staff { get; set; }
        public IEnumerable<AppointmentDto> appointments { get; set; }

    }
}