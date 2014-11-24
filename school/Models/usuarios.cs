using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace school.Models
{
    public class usuarios
    {
        public int id { get; set; }
        public string email { get; set; }
        public int fail_number { get; set; }
        public bool is_blocked { get; set; }
        public string nome { get; set; }
        public string senha { get; set; }
        public string usuario { get; set; }


    }
}