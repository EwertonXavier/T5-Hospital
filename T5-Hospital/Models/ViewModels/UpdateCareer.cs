using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models.ViewModels
{
    public class UpdateCareer
    {
        public CareerDto SelectedCareer { get; set; }
        public IEnumerable<DepartmentDto> DepartmentList { get; set; }
    }
}