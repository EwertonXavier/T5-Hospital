using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models.ViewModels
{
    public class NewAppointment
    {
        public IEnumerable<StaffDto> AvailableStaff { get; set; }
        public IEnumerable<PatientDto> AvailablePatient { get; set; }
    }
}