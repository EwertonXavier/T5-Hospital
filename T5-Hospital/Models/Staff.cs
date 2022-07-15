using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace T5_Hospital.Models
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Email { get; set; }

        //FK
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

    }
}