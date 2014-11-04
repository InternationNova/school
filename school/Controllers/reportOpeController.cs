using System;
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



        public ActionResult reportOpePdf(string escolas, List<string> producaoList)
        {
            List<string> producao = new List<string>();
            producao = producaoList;
            List<reportOpePdf> reportOpePdfArray = new List<reportOpePdf>();
            reportOpePdf reportOpePdf = new reportOpePdf();



            string esolasId = escolas;
            string producaoString = "";
            for (var i = 0; i < producao.Count; i++)
            {
                producaoString += " OR ps.id like '" + producao[i] + "'";
            }


            string str_query = @"SELECT es.nome, si.descricao, ps.id as ps_id, ps.descricao AS ps_descricao, oi.quantidade, op.numero_processo, oips.data, si.codigo_smk, previsao_embarque
			FROM ope_itens oi
			JOIN opes op ON op.id = oi.opes_id
			JOIN escolas es ON es.id = op.escolas_id
			JOIN smk_itens si ON si.id = oi.smk_itens_id
			JOIN ope_itens_producao_situacoes oips ON oips.ope_itens_id = oi.id
			JOIN producao_situacoes ps ON ps.id = oips.producao_situacoes_id
			WHERE (ps.id like @producaoStr )
			AND es.id like @escola
			AND oips.ativo =1 
			order by es.nome,ps.id, op.numero_processo";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@escola", esolasId);
                cmd.Parameters.AddWithValue("@producaoStr", producaoString);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    string numero_processo = reader["numero_processo"].ToString();
                    string codigo_smk = reader["codigo_smk"].ToString() + " - " + reader["descricao"].ToString();
                    int quantidade = Convert.ToInt32(reader["quantidade"]);
                    string ps_descricao = reader["ps_descricao"].ToString();
                    string data = reader["data"].ToString();
                    string previsao_embarque = reader["previsao_embarque"].ToString();
                    string nome = reader["nome"].ToString();

                    reportOpePdf.numero_processo = numero_processo;
                    reportOpePdf.codigo_smk = codigo_smk;
                    reportOpePdf.quantidade = quantidade;
                    reportOpePdf.ps_descricao = ps_descricao;
                    reportOpePdf.data = data;
                    reportOpePdf.previsao_embarque = previsao_embarque;
                    reportOpePdf.nome = nome;

                    reportOpePdfArray.Add(reportOpePdf);

                }

                conn.Close();
            }



            return this.ViewPdf("Customer report", "reportOpePdf", reportOpePdfArray);
        }
    }
}
