using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace school.Models
{
    public class opeVis
    {
        [Key]
        public int id { get; set; }
        public string codigo_smk { get; set; }
        public int quantidade { get; set; }
        public string valor_unitario { get; set; }
        public double valor_total { get; set; }
        public string memo { get; set; }
        public string descricao { get; set; }
        public string numero_processo { get; set; }
        public string nome { get; set; }
        public double total { get; set; }
        public string situation { get; set; }
        public string ps_descriaco { get; set; }
    }
}