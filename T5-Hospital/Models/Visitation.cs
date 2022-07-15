using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace T5_Hospital.Models
{
    public class Visitation
    {
        [Key]
        public int VisitationId { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public string RelationToVisitor { get; set; }
    }
}