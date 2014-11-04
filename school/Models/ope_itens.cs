using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace school.Models
{
    public class ope_itens
    {
        public int id { get; set; }
        public int quantidade { get; set; }
        public string previsao_embarque { get; set; }
        public double valor_unitario { get; set; }
        public double valor_total { get; set; }
        public string memo { get; set; }
        public int smk_itens_id { get; set; }
        public int opes_id { get; set; }

    }
}