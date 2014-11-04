using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace school.Models
{
    public class sub_produtos
    {
        public int id {get;set;} 
        public string descricao{get;set;}
        public int comprimento_acabada {get;set;}
        public int largura_acabada {get;set;}
        public int espessura_acabada {get;set;}
        public int comprimento_bruto {get;set;}
        public int largura_bruto {get;set;}
        public int espessura_bruto {get;set;}
        public int quantidade {get;set;}
        public decimal area {get;set;}
        public decimal perda { get; set; }
        public int smk_itens_id{get;set;}
        public int categoria_sub_produtos_id {get;set;}

    }
}