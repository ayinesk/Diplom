using LoadDist.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoadDist.Models.ViewModels
{
    public class LoadsViewModel
    {
        public int TotalLectureHours { get; set; }
        public int TotalLabsHours { get; set; }
        public int TotalPracticalHours { get; set; }
        public int TotalExamHours { get; set; }
        public int TotalTestHours { get; set; }
        public int Year { get; set; }
        public int Term { get; set; }
        public Lecturer Lecturer { get; set; }
        public List<Load> LecturerLoads { get; set; }
    }
}