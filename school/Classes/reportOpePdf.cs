using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using school.Models;

namespace school.Classes
{
    public class reportOpePdf
    {
        public string numero_processo { get; set; }
        public string codigo_smk { get; set; }
        public string descricao { get; set; }
        public int quantidade { get; set; }
        public string ps_descricao { get; set; }
        public string data { get; set; }
        public string previsao_embarque { get; set; }
        public string nome { get; set; }

    }
}
