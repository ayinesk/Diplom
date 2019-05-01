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
        public int StreamsCount { get; set; }
        public int SubgroupsCount { get; set; }
    }
}