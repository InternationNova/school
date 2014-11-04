using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using school.Models;


namespace school.Classes
{
    public class reportProdutos
    {
        public int id { get; set; }
        public List<escolas> escolas = new List<escolas>();
        public List<producao_situacoes> producao_situacoes = new List<producao_situacoes>();
    }
}