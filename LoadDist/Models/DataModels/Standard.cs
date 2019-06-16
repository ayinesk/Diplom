using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoadDist.Models.DataModels
{
    public class Standard
    {
        public int Id { get; set; }
        public decimal ExamStandard { get; set; }
        public decimal TestStandard { get; set; }
        public decimal ConsultationStandard { get; set; }
    }
}