using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using school.Controllers;
using school.Models;


namespace school.Models
{
    public class GlobalInfo : System.Data.Entity.DbContext
    {



        public DbSet<fornecedores> fornecedores { get; set; }

        public DbSet<escolas> escolas { get; set; }

        public DbSet<ope_itens> Ope_itens { get; set; }

        public DbSet<ope> Opes { get; set; }

        public DbSet<gastos> gastos { get; set; }

        public DbSet<tipo_documentos> tipo_documentos { get; set; }

        public DbSet<producao_situacoes> producao_situacoes { get; set; }

        public DbSet<estados> estados { get; set; }

        public DbSet<materia_primas> materia_primas { get; set; }

        public DbSet<transportadoras> transportadoras { get; set; }

        public DbSet<categoria_materia_primas> categoria_materia_primas { get; set; }

        public DbSet<categoria_sub_produtos> categoria_sub_produtos { get; set; }

        public DbSet<consumos> consumos { get; set; }

        public DbSet<perda> perdas { get; set; }

        public DbSet<smk_itens> smk_itens { get; set; }

        public DbSet<sub_produtos> sub_produtos { get; set; }

        public DbSet<sub_produtos_materia_primas> sub_produtos_materia_primas { get; set; }

        public DbSet<acessorios> acessorios { get; set; }

        public DbSet<ope_itens_producao_situacoes> ope_itens_producao_situacoes { get; set; }
    }
}