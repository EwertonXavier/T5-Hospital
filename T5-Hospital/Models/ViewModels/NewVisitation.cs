using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models.ViewModels
{
    public class NewVisitation
    {
        public IEnumerable<PatientDto> CurrentPatients { get; set; }
        public IEnumerable<VisitorDto> CurrentVisitors { get; set; }
    }
}