using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using school.Models;

namespace school.Classes
{
    public class opesEdit
    {
        public int id {get;set;}
        public int idvisualiza {get;set;}
        public string codigo_smk {get;set;}
        public string memo {get;set;}
        public int quantidade {get;set;}
        public double   valor_unitario {get;set;}
        public double valor_total{get;set;}
        public List<ope> opeList { get; set; }

    }
}
