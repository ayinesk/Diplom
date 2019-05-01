using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoadDist.Models.DataModels
{
    public class CourseWork
    {
        public int Id { get; set; }
        public Subject Subject { get; set; }
        public int CourseWorkHours { get; set; }
    }
}