using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoadDist.Models.ViewModels
{
    public class AddGroupViewModel
    {
        public int StreamId { get; set; }
        public int GroupId { get; set; }
    }
}