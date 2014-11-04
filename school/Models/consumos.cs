using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace school.Models
{
    
    public class consumos
    {
        public int id { get; set; }
        public double quantidade { get; set; }
        public string unidade { get; set; }
        public int materia_primas_id { get; set; }
    }
}