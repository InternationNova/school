using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using school.Classes;
using school.Models;
using System.Configuration;



namespace school.Controllers
{
    public class reportPrimaController : Controller
    {
        //
        // GET: /reportPrima/

        private GlobalInfo db = new GlobalInfo();
        // GET: /reportOpe/
        string str_connection = ConfigurationManager.ConnectionStrings["GlobalInfo"].ConnectionString;


        reportPrimaPdf reportPrimaPdfModel = new reportPrimaPdf();

        List<reportPrimaFirst> reportPrimaFirstArray = new List<reportPrimaFirst>();
        reportPrimaFirst reportPrimaFirst = new reportPrimaFirst();

        List<reportPrimaSecond> reportPrimaSecondArray = new List<reportPrimaSecond>();
        reportPrimaSecond reportPrimaSecond = new reportPrimaSecond();

        string titulo = "";
        string subtitulo = "";
        string calculo;

        public ActionResult Index()
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            reportPrimas reportPrima = new reportPrimas();
            List<escolas> escolas = new List<escolas>();

            escolas = db.escolas.ToList();
            reportPrima.escolas = escolas;

            return View(reportPrima);
        }

        public ActionResult reportPrimaPdf(List<string> selecione, List<string> codigo_smk, List<string> produto, string escolas_id, List<string> codigo_individual, List<string> produto_individual)
        {
            List<string> selection = new List<string>();
            List<string> cod_smk = new List<string>();
            List<string> produtos = new List<string>();
            List<string> cod_individual = new List<string>();
            List<string> produtos_individual = new List<string>();

            selection = selecione;
            string escolasId = escolas_id;
            string codigoIndividualSmk = "";
            string caseSwitch = "";
            string codigoString = "";
            string str_query1 = "";
            string str_query = "";
            caseSwitch = selection[0];


            for (var i = 0; i < cod_smk.Count; i++)
            {
                codigoString = " OR si.codigo_smk = " + cod_smk[i];
            }


            for (var j = 0; j < produtos_individual.Count; j++)
            {
                codigoIndividualSmk = " OR si.codigo_smk = " + cod_individual[j];
            }
            
           

            switch (caseSwitch)
            {
                case "geral":
                    str_query = @"SELECT mp.descricao, spmp.calculo, mp.codigo_smk, mp.unidade ,t1.quantidade
		                                    FROM  sub_produtos_materia_primas spmp
		                                    JOIN materia_primas mp ON mp.id = spmp.materia_primas_id
		                                    JOIN sub_produtos sp ON sp.id = spmp.sub_produtos_id
		                                    JOIN smk_itens si ON si.id = sp.smk_itens_id
		                                    JOIN ope_itens oi ON oi.smk_itens_id = si.id 
		                                    JOIN (SELECT  mp.codigo_smk ,SUM((spmp.quantidade * oi.quantidade))AS quantidade
		                                    FROM  sub_produtos_materia_primas spmp
		                                    JOIN materia_primas mp ON mp.id = spmp.materia_primas_id
		                                    JOIN sub_produtos sp ON sp.id = spmp.sub_produtos_id
		                                    JOIN smk_itens si ON si.id = sp.smk_itens_id
		                                    JOIN ope_itens oi ON oi.smk_itens_id = si.id
		                                    GROUP BY mp.codigo_smk) t1 on t1.codigo_smk = mp.codigo_smk";
                    str_query1 = @"SELECT mp.descricao, mp.unidade , mp.codigo_smk ,t1.quantidade
			                        FROM  acessorios ac
			                        JOIN smk_itens si ON si.id = ac.smk_itens_id
			                        JOIN materia_primas mp ON mp.id = ac.materia_primas_id
			                        JOIN ope_itens oi ON oi.smk_itens_id = si.id 
			
			                        JOIN (SELECT  SUM( ac.quantidade * oi.quantidade ) as quantidade , mp.codigo_smk
			                        FROM  acessorios ac
			                        JOIN smk_itens si ON si.id = ac.smk_itens_id
			                        JOIN materia_primas mp ON mp.id = ac.materia_primas_id
			                        JOIN ope_itens oi ON oi.smk_itens_id = si.id
			                        GROUP BY mp.codigo_smk) t1	on t1.codigo_smk = mp.codigo_smk";

                    titulo = "Relatório de consumo de matéria primas geral.";
                    subtitulo = "Todos os produtos";
                    break;

                case "produtos":
                    str_query = @"SELECT mp.descricao, spmp.calculo, mp.codigo_smk, mp.unidade , t1.quantidade
                                  FROM  sub_produtos_materia_primas spmp
                                  JOIN materia_primas mp ON mp.id = spmp.materia_primas_id
                                  JOIN sub_produtos sp ON sp.id = spmp.sub_produtos_id
                                  JOIN smk_itens si ON si.id = sp.smk_itens_id
                                  JOIN ope_itens oi ON oi.smk_itens_id = si.id
                                  JOIN (SELECT  SUM((spmp.quantidade * oi.quantidade))AS quantidade
                                  FROM  sub_produtos_materia_primas spmp
                                  JOIN materia_primas mp ON mp.id = spmp.materia_primas_id
                                  JOIN sub_produtos sp ON sp.id = spmp.sub_produtos_id
                                  JOIN smk_itens si ON si.id = sp.smk_itens_id
                                  JOIN ope_itens oi ON oi.smk_itens_id = si.id
                                  WHERE (si.codigo_smk like '0' " + codigoString + @")
                                  GROUP BY mp.codigo_smk) t1 on t1.codigo_smk = mp.codigo_smk
	                              WHERE (si.codigo_smk like '0' " + codigoString + @")";

                    str_query1 = @"SELECT mp.descricao, mp.unidade , mp.codigo_smk,t1.quantidade
			                        FROM  acessorios ac
			                        JOIN smk_itens si ON si.id = ac.smk_itens_id
			                        JOIN materia_primas mp ON mp.id = ac.materia_primas_id
			                        JOIN ope_itens oi ON oi.smk_itens_id = si.id
			                        Join (SELECT  SUM( ac.quantidade * oi.quantidade ) as quantidade , mp.codigo_smk
					                        FROM  acessorios ac
					                        JOIN smk_itens si ON si.id = ac.smk_itens_id
					                        JOIN materia_primas mp ON mp.id = ac.materia_primas_id
					                        JOIN ope_itens oi ON oi.smk_itens_id = si.id
					                        WHERE (si.codigo_smk like '0' " + codigoString + @") 
					                        GROUP BY mp.codigo_smk) t1 on t1.codigo_smk = mp.codigo_smk
			                        WHERE (si.codigo_smk like '0' " + codigoString + @")";
                    titulo = "Relatório de consumo de matéria primas por produtos.";
                    for (var i = 0; i < cod_smk.Count; i++)
                    {
                        subtitulo += cod_smk[i] + " - " + produtos[i] + "<br/>";
                    }

                    break;
                case "ope":
                    str_query = @"SELECT mp.descricao, spmp.calculo, mp.codigo_smk, mp.unidade ,t1.quantidade
                                   FROM sub_produtos_materia_primas spmp
                                   JOIN materia_primas mp ON mp.id = spmp.materia_primas_id
                                   JOIN sub_produtos sp ON sp.id = spmp.sub_produtos_id
                                   JOIN smk_itens si ON si.id = sp.smk_itens_id
                                   JOIN ope_itens oi ON oi.smk_itens_id = si.id 
                                   JOIN ope_itens oi ON oi.smk_itens_id = si.id
                                   JOIN (SELECT  sum((spmp.quantidade * oi.quantidade)) as quantidade , mp.codigo_smk FROM sub_produtos_materia_primas spmp JOIN materia_primas mp ON mp.id = spmp.materia_primas_id
                                   JOIN sub_produtos sp ON sp.id = spmp.sub_produtos_id
                                   JOIN smk_itens si ON si.id = sp.smk_itens_id
                                   JOIN ope_itens oi ON oi.smk_itens_id = si.id 
                                   JOIN ope_itens oi ON oi.smk_itens_id = si.id
                                   WHERE op.escolas_id ='" + escolasId + @"'
                                   GROUP BY mp.codigo_smk) t1 on t1.codigo_smk = mk.codigo_smk
   
                                   WHERE op.escolas_id ='" + escolasId + "'";
                    str_query1 = @"SELECT mp.descricao, mp.unidade, mp.codigo_smk ,t1.quantidade	
			                        FROM  acessorios ac
			                        JOIN smk_itens si ON si.id = ac.smk_itens_id
			                        JOIN materia_primas mp ON mp.id = ac.materia_primas_id
			                        JOIN ope_itens oi ON oi.smk_itens_id = si.id
			                        JOIN opes op ON op.id = oi.opes_id
			                        Join (SELECT mp.descricao, mp.unidade, SUM( ac.quantidade * oi.quantidade ) as quantidade , mp.codigo_smk
			                        FROM  acessorios ac
			                        JOIN smk_itens si ON si.id = ac.smk_itens_id
			                        JOIN materia_primas mp ON mp.id = ac.materia_primas_id
			                        JOIN ope_itens oi ON oi.smk_itens_id = si.id
			                        JOIN opes op ON op.id = oi.opes_id
			                        WHERE op.escolas_id ='" + escolasId + @"'
			                        GROUP BY mp.codigo_smk) t1 on t1.codigo_smk = mp.codigo_smk
			                        WHERE op.escolas_id ='" + escolasId + "'";

                    titulo = "Relatório de consumo de matéria primas por escola.";
                    int escolaId = Convert.ToInt32(escolasId);
                    escolas opeEscolas = db.escolas.Find(escolaId);
                    string nome = opeEscolas.nome;
                    subtitulo = nome;

                    break;

                case "individual":
                    str_query = @"SELECT mp.descricao, spmp.calculo, mp.codigo_smk, mp.unidade ,t1.quantidade			  
		                            FROM  sub_produtos_materia_primas spmp
			                              JOIN materia_primas mp ON mp.id = spmp.materia_primas_id
			                              JOIN sub_produtos sp ON sp.id = spmp.sub_produtos_id
			                              JOIN smk_itens si ON si.id = sp.smk_itens_id
			                              JOIN (SELECT mp.descricao, spmp.calculo, mp.codigo_smk, mp.unidade, sum(spmp.quantidade) as quantidade
			                              FROM  sub_produtos_materia_primas spmp
			                              JOIN materia_primas mp ON mp.id = spmp.materia_primas_id
			                              JOIN sub_produtos sp ON sp.id = spmp.sub_produtos_id
			                              JOIN smk_itens si ON si.id = sp.smk_itens_id
			                              WHERE si.codigo_smk='0' " + codigoIndividualSmk + @" GROUP BY mp.codigo_smk) t1
			                              on t1.codigo_smk = mp.codigo_smk
			                              WHERE si.codigo_smk='0' " + codigoIndividualSmk + ")";
                    str_query1 = @"SELECT mp.descricao, mp.unidade,  mp.codigo_smk ,t1.quantidade
				                    FROM  acessorios ac
				                    JOIN smk_itens si ON si.id = ac.smk_itens_id
				                    JOIN materia_primas mp ON mp.id = ac.materia_primas_id
				                    JOIN (SELECT mp.descricao, mp.unidade, sum(ac.quantidade) as quantidade, mp.codigo_smk
				                    FROM  acessorios ac
				                    JOIN smk_itens si ON si.id = ac.smk_itens_id
				                    JOIN materia_primas mp ON mp.id = ac.materia_primas_id
				                    WHERE si.codigo_smk='0' " + codigoIndividualSmk + @" GROUP BY mp.codigo_smk) t1
				                    on t1.codigo_smk = mp.codigo_smk
				                    WHERE si.codigo_smk='0' " + codigoIndividualSmk + ")";
                    titulo = "Relatório de consumo de matéria primas por produtos/individuais.";
                    for (var i = 0; i < cod_individual.Count; i++)
                    {
                        subtitulo = cod_individual[i] + " - " + produtos_individual[i] + "<br/>";
                    }
                    break;
                default:
                    if (((selection[0] == "ope") && (selection[1] == "produtos")) || (selection[1] == "ope") && (selection[0] == "produtos"))
                    {
                        str_query = @"SELECT mp.descricao, spmp.calculo, mp.codigo_smk, mp.unidade , t1.quantidade
	                                   FROM sub_produtos_materia_primas spmp
	                                   JOIN materia_primas mp ON mp.id = spmp.materia_primas_id
	                                   JOIN sub_produtos sp ON sp.id = spmp.sub_produtos_id
	                                   JOIN smk_itens si ON si.id = sp.smk_itens_id
	                                   JOIN ope_itens oi ON oi.smk_itens_id = si.id 
	                                   JOIN ope_itens oi ON oi.smk_itens_id = si.id
	                                   JOIN (SELECT mp.descricao, spmp.calculo, mp.codigo_smk, mp.unidade, sum((spmp.quantidade * oi.quantidade)) as quantidade
	                                   FROM sub_produtos_materia_primas spmp
	                                   JOIN materia_primas mp ON mp.id = spmp.materia_primas_id
	                                   JOIN sub_produtos sp ON sp.id = spmp.sub_produtos_id
	                                   JOIN smk_itens si ON si.id = sp.smk_itens_id
	                                   JOIN ope_itens oi ON oi.smk_itens_id = si.id 
	                                   JOIN ope_itens oi ON oi.smk_itens_id = si.id
	                                   WHERE op.escolas_id ='" + escolasId + "' and (si.codigo_smk like '0' " + codigoString + @")
	                                   GROUP BY mp.codigo_smk) t1 on join t1.codigo_smk = mp.codigo_smk
	                                   WHERE op.escolas_id ='" + escolasId + "' and (si.codigo_smk like '0' " + codigoString + ")";
                        str_query1 = @"SELECT mp.descricao , mp.unidade,  mp.codigo_smk , t1.quantidade
			                            FROM  acessorios ac
			                            JOIN smk_itens si ON si.id = ac.smk_itens_id
			                            JOIN materia_primas mp ON mp.id = ac.materia_primas_id
			                            JOIN ope_itens oi ON oi.smk_itens_id = si.id
			                            JOIN opes op ON op.id = oi.opes_id
			                            JOIN (	SELECT mp.descricao, mp.unidade, SUM( ac.quantidade * oi.quantidade ) as quantidade , mp.codigo_smk
			                            FROM  acessorios ac
			                            JOIN smk_itens si ON si.id = ac.smk_itens_id
			                            JOIN materia_primas mp ON mp.id = ac.materia_primas_id
			                            JOIN ope_itens oi ON oi.smk_itens_id = si.id
			                            JOIN opes op ON op.id = oi.opes_id
			                            WHERE op.escolas_id ='" + escolasId + "' and (si.codigo_smk like '0' " + escolasId + @")
			                            GROUP BY mp.codigo_smk) t1 on t1.codigo_smk = mp.codigo_smk
			                            WHERE op.escolas_id ='" + escolasId + "' and (si.codigo_smk like '0' " + escolasId + ")";

                        titulo = "Relatório de consumo de matéria primas por escola e produto.";
                        titulo = "Relatório de consumo de matéria primas por escola.";
                        escolaId = Convert.ToInt32(escolasId);
                        opeEscolas = db.escolas.Find(escolaId);
                        nome = opeEscolas.nome;
                        subtitulo = nome;
                        for (var i = 0; i < cod_smk.Count; i++)
                        {
                            subtitulo += subtitulo = cod_smk[i] + " - " + produtos[i] + "<br/>";
                        }



                    }


                    break;

            }
            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    string codigo_smks = reader["codigo_smk"].ToString();
                    string descricao = reader["descricao"].ToString();

                    string unidade = reader["unidade"].ToString();
                    int quantidade = Convert.ToInt32(reader["quantidade"]);
                    calculo = reader["calculo"].ToString();

                    reportPrimaFirst.codigo_smk = codigo_smks + "-" + descricao;
                    reportPrimaFirst.unidade = unidade;
                    if (calculo == "mdf")
                    {
                        reportPrimaFirst.mdf = Math.Round((quantidade / 5.0325), 3);
                    }
                    else if (calculo == "laminado")
                    {
                        reportPrimaFirst.mdf = Math.Round((quantidade / 3.85), 3);
                    }
                    else
                    {

                        reportPrimaFirst.mdf = quantidade;
                    }
                    reportPrimaFirstArray.Add(reportPrimaFirst);

                }

                conn.Close();
            }

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query1, conn))
            {

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    string codigo_smks = reader["codigo_smk"].ToString();
                    string descricao = reader["descricao"].ToString();

                    string unidade = reader["unidade"].ToString();
                    int quantidade = Convert.ToInt32(reader["quantidade"]);
                    

                    reportPrimaSecond.codigo_smk = codigo_smks + "-" + descricao;
                    reportPrimaSecond.unidade = unidade;

                    if (calculo == "mdf")
                    {
                        reportPrimaSecond.mdf = Math.Round((quantidade / 5.0325), 3);
                    }
                    else if (calculo == "laminado")
                    {
                        reportPrimaSecond.mdf = Math.Round((quantidade / 3.85), 3);
                    }
                    else
                    {

                        reportPrimaSecond.mdf = quantidade;
                    }
                    reportPrimaSecondArray.Add(reportPrimaSecond);

                }
                reportPrimaPdfModel.reportPrimaFirst = reportPrimaFirstArray;
                reportPrimaPdfModel.reportPrimaSecond = reportPrimaSecondArray;
                reportPrimaPdfModel.titulo = titulo;
                reportPrimaPdfModel.subtitulo = subtitulo;

                conn.Close();
            }


            return View(reportPrimaPdfModel);
        }

    }
}

