using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models.ViewModels
{
    public class UpdateVisitation
    {
        public VisitationDto VisitationRecord { get; set; }
        public IEnumerable<PatientDto> CurrentPatients { get; set; }
        public IEnumerable<VisitorDto> CurrentVisitors { get; set; }
    }
}