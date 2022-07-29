using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models.ViewModels
{
    public class DetailsDepartment
    {
        public IEnumerable<ServiceDto> Services { get; set; }
        public DepartmentDto Department { get; set; }
    }
}