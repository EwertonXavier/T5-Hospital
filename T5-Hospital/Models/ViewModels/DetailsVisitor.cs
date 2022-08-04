using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models.ViewModels
{
    public class DetailsVisitor
    {
        public VisitorDto SelectedVisitor { get; set; }
        public IEnumerable<VisitationDto> PatientsVisiting { get; set; }
    }
}