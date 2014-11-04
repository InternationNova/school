using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using school.Models;

namespace school.Classes
{
    public class smkAccessory
    {
        public int id { get; set; }
        public string codigo_smk { get; set; }
        public int cmp_id { get; set; }
        public string unidade { get; set; }
        public decimal quantitide { get; set; }
        public int sub_produtos_id { get; set; }
        public string categoria { get; set; }
        public int materia_primas_id { get; set; }


    }
}