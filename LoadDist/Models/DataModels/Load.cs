using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoadDist.Models.DataModels
{
    public class Load
    {
        public int Id { get; set; }
        public Lecturer Lecturer { get; set; }
        public Subject Subject { get; set; }
        public SyllabusContent SyllabusContent { get; set; }
        public int LectureHours { get; set; }
        public int LabsHours { get; set; }
        public int PracticalHours { get; set; }
        public int ExamHours { get; set; }
        public int TestHours { get; set; }
        public int Year { get; set; }
        public int Term { get; set; }
    }
}