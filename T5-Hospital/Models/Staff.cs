using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace T5_Hospital.Models
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }
        int DepartmentId{ get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
    }
}