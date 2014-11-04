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

    public class reportComprasController : PdfViewController
    {
        //
        // GET: /reportCompras/
        private GlobalInfo db = new GlobalInfo();
        // GET: /reportOpe/
        string str_connection = ConfigurationManager.ConnectionStrings["GlobalInfo"].ConnectionString;

        public ActionResult Index()
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        public ActionResult reportComprasPdf(List<string> memo)
        {
            List<string> memos = new List<string>();
            memos = memo;

            reportComprasPdf reportComprasPdf = new reportComprasPdf();
            List<reportComprasFirst> reportComprasFirstArray = new List<Classes.reportComprasFirst>();
            List<reportComprasSecond> reportComprasSecondArray = new List<reportComprasSecond>();

            reportComprasFirst reportComprasFirst = new reportComprasFirst();
            reportComprasSecond reportComprasSecond = new reportComprasSecond();

            string memoString = "";

            for (var i = 0; i < memos.Count; i++)
            {
                memoString += " OR oi.memo=" + memos[i];
            }
            string addingSql = "";

            string str_query = @"SELECT t1.*, t2.quantidade
                                  FROM 
                                    (SELECT oi.memo,
                                           mp.codigo_smk,
                                           mp.descricao,
                                           mp.unidade,
                                           si.codigo_smk AS codigo_smk1,
                                           si.descricao AS descricao1,
                                           spmp.calculo
                                    FROM ope_itens oi
                                    JOIN smk_itens si ON si.id = oi.smk_itens_id
                                    JOIN sub_produtos sp ON sp.smk_itens_id = si.id
                                    JOIN sub_produtos_materia_primas spmp ON spmp.sub_produtos_id = sp.id
                                    JOIN materia_primas mp ON mp.id = spmp.materia_primas_id WHERE (oi.memo like '0'" + memoString +
                                    ")) t1," + @"(SELECT oi.memo,
                                           mp.codigo_smk,
                                           (SUM(spmp.quantidade * oi.quantidade)) AS quantidade
                                    FROM ope_itens oi
                                    JOIN smk_itens si ON si.id = oi.smk_itens_id
                                    JOIN sub_produtos sp ON sp.smk_itens_id = si.id
                                    JOIN sub_produtos_materia_primas spmp ON spmp.sub_produtos_id = sp.id
                                    JOIN materia_primas mp ON mp.id = spmp.materia_primas_id WHERE (oi.memo like '0'" + memoString + @")
                                        GROUP BY oi.memo, mp.codigo_smk) t2
                                     WHERE t1.memo = t2.memo
                                       AND t1.codigo_smk = t2.codigo_smk
                                     ORDER BY t1.descricao";




            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string memoElement = reader["memo"].ToString();
                    string codigo_smk = reader.GetString(1) + "-" + reader.GetString(2);
                    string unidade = reader["unidade"].ToString();
                    double quantade;
                    int quantidade = Convert.ToInt32(reader["quantidade"]);
                    string calculo = reader["calculo"].ToString();
                    string codigo_smk1 = reader.GetString(5) + "-" + reader.GetString(6);

                    if ((calculo == "mdf" || calculo == "laminado"))
                    {
                        unidade = "CH";
                    }
                    if (calculo == "mdf")
                    {

                        quantade = quantidade / 5.0325;
                        quantade = Math.Round(quantade, 3);

                    }
                    else if (calculo == "laminado")
                    {
                        quantade = quantidade / 3.85;
                        quantade = Math.Round(quantade, 3);
                    }
                    else
                    {

                        quantade = quantidade;
                    }

                    reportComprasFirst.memo = memoElement;
                    reportComprasFirst.codigo_smk = codigo_smk;
                    reportComprasFirst.unidade = unidade;
                    reportComprasFirst.quantidade = quantade;
                    reportComprasFirst.codigo_smk1 = codigo_smk1;
                    reportComprasFirstArray.Add(reportComprasFirst);

                }

                conn.Close();
            }
            reportComprasPdf.reportComprasFirst = reportComprasFirstArray;
            str_query = @"SELECT t1.*, t2.quantidade
                              FROM 
                            (SELECT oi.memo, 
		                            mp.codigo_smk, 
		                            mp.descricao, 
		                            mp.unidade, 
		
		                            si.codigo_smk AS codigo_smk1,
		                            si.descricao AS descriaco1
                            FROM ope_itens oi
                            JOIN smk_itens si ON si.id = oi.smk_itens_id
                            JOIN acessorios ac ON ac.smk_itens_id = si.id
                            JOIN materia_primas mp ON mp.id = ac.materia_primas_id
                            WHERE (oi.memo like '0'" + memoString + @")) t1 ,

                            (SELECT oi.memo,
		                            mp.codigo_smk,
		                            SUM(ac.quantidade*oi.quantidade) AS quantidade
                            FROM ope_itens oi
                            JOIN smk_itens si ON si.id = oi.smk_itens_id
                            JOIN acessorios ac ON ac.smk_itens_id = si.id
                            JOIN materia_primas mp ON mp.id = ac.materia_primas_id
                            WHERE (oi.memo like '0'" + memoString + @")
                            GROUP BY oi.memo,
                                     mp.codigo_smk ) t2 
                            WHERE t1.memo = t2.memo AND t1.codigo_smk = t2.codigo_smk 
                            ORDER BY t1.descricao";



            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {


                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    string memoElement = reader["memo"].ToString();
                    string codigo_smk = reader.GetString(1) + "-" + reader.GetString(2);
                    string unidade = reader["unidade"].ToString();
                    string descricao = reader["descricao"].ToString();
                    int quantidade = Convert.ToInt32(reader["quantidade"]);
                    string calculo = reader["calculo"].ToString();
                    string codigo_smk1 = reader.GetString(5) + "-" + reader.GetString(6);

                    reportComprasSecond.memo = memoElement;
                    reportComprasSecond.codigo_smk = codigo_smk;
                    reportComprasSecond.unidade = unidade;
                    reportComprasSecond.descricao = descricao;
                    reportComprasSecond.codigo_smk1 = codigo_smk1;

                    reportComprasSecondArray.Add(reportComprasSecond);
                }

                conn.Close();
            }
            reportComprasPdf.reportComprasFirst = reportComprasFirstArray;
            reportComprasPdf.reportComprasSecond = reportComprasSecondArray;

            return this.ViewPdf("Customer report", "reportComprasPdf", reportComprasPdf);
        }
    }
}
