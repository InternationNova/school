using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using school.Models;
using System.Data.SqlClient;
using school.Classes;
using System.Configuration;


namespace school.Controllers
{
    public class reportOpeController : PdfViewController
    {
        private GlobalInfo db = new GlobalInfo();
        // GET: /reportOpe/
        string str_connection = ConfigurationManager.ConnectionStrings["GlobalInfo"].ConnectionString;

        public ActionResult Index()
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            reportOpes reportOpes = new reportOpes();
            List<escolas> escolas = new List<escolas>();
            List<producao_situacoes> producao_situacoes = new List<producao_situacoes>();

            escolas = db.escolas.ToList();
            producao_situacoes = db.producao_situacoes.ToList();

            reportOpes.escolas = escolas;
            reportOpes.producao_situacoes = producao_situacoes;

            return View(reportOpes);
        }

        public ActionResult reportOpePdfView(string escolasNew, string producaoListString)
        {
            List<string> producaoInt = new List<string>();
            List<int> producaoInteger = new List<int>();
            List<reportOpePdf> reportOpePdfArray = new List<reportOpePdf>();
            reportOpePdf reportOpePdf = new reportOpePdf();



            string[] strArr = producaoListString.Split('/');
            //String[] listBuffer = (String[]) producaoListNew;
                        
            int esolasId = Convert.ToInt32(escolasNew);

            for (var i = 0; i < strArr.Length-1; i++)
            {
                producaoInteger.Add(Convert.ToInt32(strArr[i]));
            }

            reportOpePdfArray = (from x in db.Ope_itens
                                 join op in db.Opes on x.opes_id equals op.id
                                 join es in db.escolas on op.escolas_id equals es.id
                                 join si in db.smk_itens on x.smk_itens_id equals si.id
                                 join oips in db.ope_itens_producao_situacoes on x.id equals oips.ope_itens_id
                                 join ps in db.producao_situacoes on oips.producao_situacoes_id equals ps.id
                                 where producaoInteger.Contains(ps.id) && es.id == esolasId && oips.ativo == 1
                                 select new reportOpePdf
                                 {
                                     numero_processo = op.numero_processo,
                                     codigo_smk = si.codigo_smk,
                                     descricao = si.descricao,
                                     quantidade = x.quantidade,
                                     ps_descricao = ps.descricao,
                                     data = oips.data,
                                     previsao_embarque = x.previsao_embarque,
                                     nome = es.nome

                                 }).ToList();

            return View(reportOpePdfArray);
        }

        public ActionResult reportOpePdf(string escolas, List<string> producaoList)
        {
            string escolasNew = escolas;
//            List<string> producaoListNew = producaoList;
            //return new Rotativa.ActionAsPdf("reportOpePdfView", Object obj);

            string producaoListString = "" ;
            for (int i = 0; i < producaoList.Count; i++)
                producaoListString = producaoListString +  producaoList[i] + "/";

            return new Rotativa.ActionAsPdf("reportOpePdfView", new { escolasNew, producaoListString });
       	
            //return View(reportOpePdfArray);
        }
    }
}
