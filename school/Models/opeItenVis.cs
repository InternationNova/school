using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
namespace school.Models
{
    public class opeItenVis
    {
        [Key]
        public int id { get; set; }
        public ope_itens ope_itens { get; set; }
        public List<ope_itens_producao_situacoes> ope_itens_producao_situacoes { get; set; }
        public List<gastos> gastos { get; set; }
    }
}