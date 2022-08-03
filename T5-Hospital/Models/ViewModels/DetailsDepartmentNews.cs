using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models.ViewModels
{
    public class DetailsDepartmentNews
    {
        public IEnumerable<NewsDto> News { get; set; }
        public DepartmentDto Department { get; set; }
    }
}