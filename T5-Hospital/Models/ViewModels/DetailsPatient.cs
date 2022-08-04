using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models.ViewModels
{
    public class DetailsPatient
    {
        public PatientDto SelectedPatient { get; set; }
        public IEnumerable<AppointmentDto> Appointments { get; set; }
        public IEnumerable<VisitationDto> Visitors { get; set; }
    }
}