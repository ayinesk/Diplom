using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoadDist.Models.DataModels
{
    public class Stream
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Group> Groups { get; set; }
    }
}