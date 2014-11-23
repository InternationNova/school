using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace school.Models
{
    public class acessorios
    {
        public int id { get; set; }
        public int smk_itens_id { get; set; }
        public int materia_primas_id { get; set; }
        public double quantidade { get; set; }

    }

    public class acessoriosEdit {

        public int id { get; set; }
        public int smk_itens_id { get; set; }
        public int materia_primas_id { get; set; }
        public double quantidade { get; set; }
        public string descricao { get; set; }
        public int idSmkItem { get; set; }
         
    }
}