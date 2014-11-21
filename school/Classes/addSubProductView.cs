using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using school.Models;

namespace school.Classes
{
    public class addSubProductView
    {
        public sub_produtos sub_produtosObj { get; set; }
        public smk_itens smk_itensObj {get;set;}
        public List<categoria_sub_produtos> categoria_sub_produtosArr { get; set; }
        public int id { get; set; }
        public int codigoSmk{get;set; }
        
    }
}