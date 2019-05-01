using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoadDist.Models.DataModels
{
    public class Group
    {
        public int Id { get; set; }
        public string GroupNumber { get; set; }
        public int StudentsCount { get; set; }
        public Syllabus Syllabus { get; set; }
        public int SubgroupsCount { get; set; }
    }
}