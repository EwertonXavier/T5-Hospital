using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models.ViewModels
{
    public class DetailsNews
    {
        public IEnumerable<NewsDto> NewsData { get; set; }
        public DepartmentDto Department { get; set; }
    }
}