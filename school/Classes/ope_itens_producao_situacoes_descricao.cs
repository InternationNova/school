using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace school.Classes
{
    public class ope_itens_producao_situacoes_descricao
    {
        public int id { get; set; }
        public int ope_itens_id { get; set; }
        public int producao_situacoes_id { get; set; }
        public string data { get; set; }
        public int ativo { get; set; }
        public string descricao { get; set; }
    }
}