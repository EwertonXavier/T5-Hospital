using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models.ViewModels
{
    public class UpdateService
    {
        public ServiceDto SelectedService { get; set; }
        public IEnumerable<DepartmentDto> AvailableDepartments{ get; set; }
    }
}