using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace school.Classes
{
    public class gastosItemDetail
    {
        public int id { get; set; }
        public double total { get; set; }
        public string data { get; set; }
        public string numero_documento { get; set; }
        public int memo { get; set; }
        public int fornecedores_id { get; set; }
        public int tipo_documentos_id { get; set; }
        public int ope_itens_id { get; set; }
        public string nome { get; set; }
    }
}