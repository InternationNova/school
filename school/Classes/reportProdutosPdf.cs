using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace school.Classes
{
    public class reportProdutosPdf
    {
        public string escola { get; set; }
        public string codigo_smk { get; set; }
        public string descricao { get; set; }
        public string memo { get; set; }
        public double totalProdutos { get; set; }
        public double amount { get; set; }
        public int quantidade { get; set; }
        public double totalSubProdutos { get; set; }
    }
}