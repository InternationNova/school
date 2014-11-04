using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace school.Models
{
    public class ope_itens_producao_situacoes
    {
        public int id { get; set; }
        public int ope_itens_id { get; set; }
        public int producao_situacoes_id { get; set; }
        public string data { get; set; }
        public int ativo { get; set; }
    }
}