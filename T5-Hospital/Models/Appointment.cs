using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace T5_Hospital.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        //int departmentId{ get; set; }
        public string name { get; set; }

        public string email { get; set; }
    }
}