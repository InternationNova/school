using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using school.Classes;
using school.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace school.Controllers
{
    public class reportProdutosController : PdfViewController
    {
        //
        // GET: /reportProdutos/
        private GlobalInfo db = new GlobalInfo();
        string str_connection = ConfigurationManager.ConnectionStrings["GlobalInfo"].ConnectionString;

        List<escolas> escolas = new List<escolas>();

        public ActionResult Index()
        {

            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            reportProdutos reportProdutos = new reportProdutos();

            List<producao_situacoes> producao_situacoes = new List<producao_situacoes>();
            escolas = db.escolas.ToList();
            producao_situacoes = db.producao_situacoes.ToList();
            reportProdutos.escolas = escolas;
            reportProdutos.producao_situacoes = producao_situacoes;

            return View(reportProdutos);
        }
        public ActionResult getEscolas()
        {

            escolas = db.escolas.ToList();

            return View(escolas);
        }
        public ActionResult buscaItem(string cod_smk, string op)
        {
            
            smk_itens smk_itemObj = new smk_itens();
            switch (op)
            {
                case "item_smk":
                    string conteudo1 = "0";
                    smk_itemObj = (from x in db.smk_itens
                                   where x.codigo_smk == cod_smk
                                   select x).FirstOrDefault();

                    if (smk_itemObj != null )
                    {
                        
                        conteudo1 = smk_itemObj.descricao;
                        return Json(new { conteudo = conteudo1 });
                    }
                    else
                    {

                        return Json(new { conteudo = "0" });
                    }

                    break;
          
                default:
                    return Json(new { conteudo = "Falta parâmetros" });
                    break;


            }

            if (Session["USERNAME"] == null)
            {
                return Json(new { ok = "failed" });

            }
            else
            {
                return Json(new { ok = "success" });
            }


        }
        public ActionResult reportProdutosPdf(string producao_situacao_id, List<string> escolas_id, List<string> codigo_smk, List<string> produto)
        {
            List<string> producao = new List<string>();
            string producao_situacao = producao_situacao_id;
            List<string> escolasId = escolas_id;
            List<string> codigoSmk = codigo_smk;

            	
	        string codigo_smkArr = "";
            string codigo_escola = "";
            string titulo = "Relatórios ref. a produtos";
	        string subtituloSituacao = "" ;
            string  subtituloEscola = "" ;
            string subtituloSmk = "";
            double totalProdutos = 0;
	      
            string str_query = @"SELECT codigo_smk, es.nome as escola, si.descricao as si_descricao, ps.descricao as ps_descricao, memo, ps.id, SUM( quantidade ) as quantidade FROM ope_itens oi 
			JOIN opes op on op.id=oi.opes_id JOIN escolas es on es.id=op.escolas_id JOIN smk_itens si ON si.id = oi.smk_itens_id JOIN ope_itens_producao_situacoes oips ON oips.ope_itens_id = oi.id JOIN producao_situacoes ps ON ps.id = oips.producao_situacoes_id WHERE";

            str_query = str_query + "ps.id = " + producao_situacao;

            string add_query = "";
            string added_query = "";

            for (var i = 0; i < escolasId.Count; i++)
            {
                add_query += " OR ps.id like '" + escolasId[i] + "'";
            }
            add_query = " and (si.codigo_smk = '0' " + add_query + ")";

            for (var i = 0; i < codigoSmk.Count; i++)
            {
                added_query += " OR si.codigo_smk like '%" + codigoSmk[i] + "%'";
            }
            add_query = " and (si.codigo_smk = '0' " + added_query + ")";
            add_query += " group by oi.memo order by si.escola";

            str_query = str_query + add_query;


            List<reportProdutosPdf> reportProdutosPdfArray = new List<reportProdutosPdf>();

            reportProdutosPdf reportProdutosPdf = new reportProdutosPdf();

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    string escola = reader["escola"].ToString();
                    string codigosmk = reader["codigo_smk"].ToString() + " - " + reader["descricao"].ToString();
                    string memo = reader["memo"].ToString();
                    int quantidade = Convert.ToInt32(reader["quantidade"]);
                    string codigosmkReal = reader["codigo_smk"].ToString();

                    string prodAnterior = "";
                    int quantidadeAnterior = 0;
                    double totalSubProdutos = new double();
                    bool inicioSoma = true;


                    if (codigosmkReal == prodAnterior)
                    {
                        if (inicioSoma == true)
                        {
                            totalSubProdutos += quantidadeAnterior;
                            inicioSoma = false;
                        }
                        totalSubProdutos += quantidadeAnterior;
                    }
                    else
                    {
                        totalSubProdutos = 0;
                        inicioSoma = true;
                    }
                    prodAnterior = codigosmkReal;
                    quantidadeAnterior = quantidade;

                    double sum = reader.GetDouble(6);

                    reportProdutosPdf.escola = escola;
                    reportProdutosPdf.codigo_smk = codigosmk;
                    reportProdutosPdf.memo = memo;
                    reportProdutosPdf.amount = sum;
                    reportProdutosPdf.totalProdutos = totalSubProdutos + quantidade;
                    reportProdutosPdf.totalSubProdutos = totalSubProdutos;

                    reportProdutosPdfArray.Add(reportProdutosPdf);

                }

                conn.Close();
            }

            return this.ViewPdf("Customer report", "reportProdutosPdf", reportProdutosPdfArray);
        }
        
    }
}
