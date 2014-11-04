using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace school.Models
{
    public class materia_primas
    {
        public int id { get; set; }
        public string codigo_smk { get; set; }
        public string descricao { get; set; }
        public string unidade { get; set; }
        public int categoria_materia_primas_id { get; set; }
    }
}