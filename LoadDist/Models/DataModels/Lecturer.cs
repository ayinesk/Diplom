using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoadDist.Models.DataModels
{
    public class Lecturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public List<Subject> Subjects { get; set; }
    }
}