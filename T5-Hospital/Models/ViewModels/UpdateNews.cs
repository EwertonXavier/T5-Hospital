using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models.ViewModels
{
    public class UpdateNews
    {
        public NewsDto SelectedNews { get; set; }
        public IEnumerable<DepartmentDto> RelatedDepartments { get; set; }
    }
}