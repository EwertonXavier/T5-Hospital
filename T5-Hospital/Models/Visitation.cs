using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    
        // Foreign Keys
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        [ForeignKey("Visitor")]
        public int VisitorId { get; set; }
        public virtual Visitor Visitor { get; set; }
    }
    public class VisitationDto
    {
        public int VisitationId { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public string RelationToVisitor { get; set; }
        // Fields accessed through the virtual/foreign key members
        public int PatientId { get; set; }
        public string PatientFirstName { get; set; }
        public int VisitorId { get; set; }
        public string VisitorFirstName { get; set; }
    }
}