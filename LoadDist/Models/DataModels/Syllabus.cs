using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoadDist.Models.DataModels
{
    public class Syllabus
    {
        public int Id { get; set; }
        public Specialty Specialty { get; set; }
        public int AdmissionYear { get; set; }
    }
}