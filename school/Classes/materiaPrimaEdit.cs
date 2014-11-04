using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using school.Models;

namespace school.Classes
{
    public class materiaPrimaEdit
    {
        public int id { get; set; }
        public string codigo_smk { get; set; }
        public string descricao { get; set; }
        public string unidade { get; set; }
        public int categoria_materia_primas_id { get; set; }
        public List<categoria_materia_primas> categoria_materia_primas { get; set; }
    }
}