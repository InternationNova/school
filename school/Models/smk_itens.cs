using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using school.Models;


namespace school.Models
{
    public class smk_itens
    {
        public int id { get; set; }
        public string codigo_smk { get; set; }
        public int codigo_antigo { get; set; }
        public string descricao { get; set; }
    }
}