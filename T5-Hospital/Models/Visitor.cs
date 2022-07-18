using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models
{
    public class Visitor
    {
        [Key]
        public int VisitorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class VisitorDto
    {
        public int VisitorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}