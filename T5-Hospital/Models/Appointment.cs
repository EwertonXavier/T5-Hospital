﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace T5_Hospital.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public int StaffId { get; set; }
        public int PatientId { get; set; }

        public string StartDate { get; set; }
    }
}