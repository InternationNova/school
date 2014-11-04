using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using school.Models;

namespace school.Classes
{
    public class smkSubProdus
    {
        public int id { get; set; }
        public string sp_descricao { get; set; }
        public string comprimento_acabada { get; set; }
        public string espessura_acabada { get; set; }
        public string comprimento_bruto { get; set; }
        public string largura_acabada { get; set; }
        public string largura_bruto { get; set; }
        public string espessura_bruto { get; set; }
        public double quantidade { get; set; }
        public string area { get; set; }
        public string perda { get; set; }
        public int smk_itens_id { get; set; }
        public string csp_descricao { get; set; }
        public smk_itens smk_itens { get; set; }
        public List<smkAccessory> smkAccessory { get; set; }
    }

}