using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models.ViewModels
{
    public class UpdateStaff
    {
        public IEnumerable<DepartmentDto> AvailableDepartments { get; set; }
        public StaffDto SelectedStaff { get; set; }
    }
}