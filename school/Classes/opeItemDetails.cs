using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using school.Models;
using System.ComponentModel.DataAnnotations;

namespace school.Classes
{
    public class opeItemDetails
    {
        public int id { get; set; }
        public int quantidade { get; set; }
        public double valor_unitario { get; set; }
        public string codigo_smk { get; set; }
        public double valor_total { get; set; }
        public string memo { get; set; }
        public string descricao { get; set; }
        public string numero_processo { get; set; }
        public string nome { get; set; }
        public string previsao_embarque { get; set; }
        public string item { get; set; }

        public List<ope_itens_producao_situacoes_descricao> ope_itens { get; set; }
        public List<gastosItemDetail> gastos { get; set; }

    }
}