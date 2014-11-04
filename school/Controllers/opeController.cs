using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using school.Models;
using System.Data.SqlClient;
using school.Classes;
using System.Configuration;

namespace school.Controllers
{
    public class opeController : Controller
    {
        private GlobalInfo db = new GlobalInfo();

        string str_connection = ConfigurationManager.ConnectionStrings["GlobalInfo"].ConnectionString; //
        // GET: /Ope/

        public ActionResult Index()
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<opeIndex> arrModel = new List<opeIndex>();
            string str_query = "select op.id, numero_processo,  data_abertura, data_chegada, es.nome from opes op join escolas es on es.id=op.escolas_id";
            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string nome = reader["nome"].ToString();
                    string numero_processo = reader["numero_processo"].ToString();

                    string data_abertura = reader["data_abertura"].ToString();
                    string data_chegada = reader["data_chegada"].ToString();
                    int nId = Convert.ToInt32(reader["id"]);
                    opeIndex objOPE = new opeIndex();
                    ope objope = new ope();

                    objope.numero_processo = numero_processo;

                    objope.data_abertura = data_abertura;
                    objope.data_chegada = data_chegada;
                    objOPE.nome = nome;
                    objOPE.id = nId;
                    objOPE.ope = objope;
                    /////// opevis model

                    arrModel.Add(objOPE);

                }

                conn.Close();
            }
            return View(arrModel);

        }

        public ViewResult DetailsVis(int id)
        {
            opeDetails arrModel = new opeDetails();
            List<opeVis> opeVis = new List<opeVis>();

            string str_query = "select op.id, numero_processo, numero_projeto, es.nome, escolas_id from opes op join escolas es on es.id = op.escolas_id where op.id = @id";
            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int nid = Convert.ToInt32(reader["id"]);
                    string numero_processo = reader["numero_processo"].ToString();
                    string numero_projeto = reader["numero_projeto"].ToString();
                    string nome = reader["nome"].ToString();
                    int escolas_id = Convert.ToInt32(reader["escolas_id"]);
                    opeVis = getopeVisList(nid);
                    arrModel.opeVis = opeVis;
                    arrModel.nome = nome;
                    arrModel.numero_processo = numero_processo;
                    arrModel.numero_projeto = numero_projeto;
                    arrModel.id = nid;

                }

                conn.Close();
            }

            return View(arrModel);
        }
      
      

        /////get descriaco
       

        ///get opelist in details page
        public List<opeVis> getopeVisList(int id)
        {

            List<opeVis> opeVisList = new List<opeVis>();
            
            string str_query = @"SELECT oi.id, si.codigo_smk, quantidade, valor_unitario, valor_total, si.descricao, numero_processo, es.nome ,t1.memo ,t1.tot 	
	                                FROM  ope_itens oi
	                                JOIN smk_itens si ON si.id = oi.smk_itens_id
	                                JOIN opes op ON op.id = oi.opes_id
	                                JOIN escolas es ON es.id = op.escolas_id
	                                JOIN(SELECT oi.memo , SUM( ga.total ) AS tot 
		                                FROM  ope_itens oi
		                                 LEFT JOIN gastos ga ON ga.ope_itens_id = oi.id
	         
	                                         GROUP BY oi.memo) t1 ON t1.memo = oi.memo 
	                                 WHERE op.id = @id ";
            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int nId = Convert.ToInt32(reader["id"]);
                    string codigo_smk = reader["codigo_smk"].ToString();
                    int quantidade = Convert.ToInt32(reader["quantidade"]);
                    string descricao = reader["descricao"].ToString();
                    string memo = reader["memo"].ToString();
                    double tot;
                    if (String.IsNullOrEmpty(reader["tot"].ToString()))
                    {
                        tot = 0;
                    }
                    else
                    {
                        tot = Convert.ToDouble(reader["tot"]);
                    }
                    double saldo = Convert.ToDouble(reader["valor_total"]) - tot;
                    string situation = getSituation(nId);
                    opeVis opeVis = new opeVis();
                    opeVis.id = nId;
                    opeVis.codigo_smk = codigo_smk;
                    opeVis.descricao = descricao;
                    opeVis.quantidade = quantidade;
                    opeVis.memo = memo;
                    opeVis.total = tot;
                    opeVis.valor_total = saldo;
                    opeVis.situation = situation;
                    opeVisList.Add(opeVis);
                    opeVis = null;

                }

                conn.Close();
            }
            return opeVisList;
        }

        public string getSituation(int id)
        {
            string situation = "";
            string str_query = @"SELECT ps.descricao AS ps_descricao
										FROM ope_itens_producao_situacoes oips
									    JOIN producao_situacoes ps ON ps.id = oips.producao_situacoes_id
									    JOIN ope_itens oi ON oi.id = oips.ope_itens_id
										WHERE oi.id = @id
										order by oips.id desc";
            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    situation = reader["ps_descricao"].ToString();
                }

            }

            return situation;
        }
        //
        // GET: /Ope/Details/5

        public ViewResult Details(int id)
        {
            ope ope = db.Opes.Find(id);

            return View(ope);
        }

        public ViewResult DetailsView(int id)
        {
            List<opeVisual> arrModel = new List<opeVisual>();
            List<opeVis> arrVisModel = new List<opeVis>();
            string str_query = "SELECT op.id, numero_processo, numero_projeto, es.nome, escolas_id FROM opes op JOIN escolas es ON es.id=op.escolas_id WHERE op.id=@id";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int nId = Convert.ToInt32(reader["id"]);
                    string numero_processo = reader["numero_processo"].ToString();
                    string numero_projeto = reader["numero_projeto"].ToString();
                    string nome = reader["nome"].ToString();
                    int escolas_id = Convert.ToInt32(reader["escolas_id"]);

                    opeVisual objOPE = new opeVisual();
                    objOPE.id = nId;
                    objOPE.numero_processo = numero_processo;
                    objOPE.numero_projeto = numero_projeto;
                    objOPE.nome = nome;
                    objOPE.escolas_id = escolas_id;

                    /////// opevis model

                    objOPE.opeVisualier = getopeVis(id);
                    arrModel.Add(objOPE);

                }

                conn.Close();
            }

            return View(arrModel);
        }
        public List<gastos> get_gastos(int id)
        {
            List<gastos> visModel = new List<gastos>();
            string str_query = "select ga.id, ga.total, fo.nome, data, numero_documento from gastos ga " +
              " join fornecedores fo on fo.id = ga.fornecedores_id where ga.ope_itens_id = @id";
            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int nId = Convert.ToInt32(reader["id"]);
                    decimal total = Convert.ToDecimal(reader["total"]);
                    string data = reader["data"].ToString();
                    string numero_documento = reader["numero_documento"].ToString();
                    int memo = Convert.ToInt32(reader["memo"]);
                    int fornecedores_id = Convert.ToInt32(reader["fornecedores_id"]);
                    int tipo_documentos_id = Convert.ToInt32(reader["tipo_documentos_id"]);
                    int ope_itens_id = Convert.ToInt32(reader["ope_itens_id"]);

                    gastos objVis = new gastos();
                    objVis.id = nId;

                    objVis.data = data;
                    objVis.numero_documento = numero_documento;
                    objVis.memo = memo;
                    objVis.fornecedores_id = fornecedores_id;
                    objVis.tipo_documentos_id = tipo_documentos_id;
                    objVis.ope_itens_id = ope_itens_id;

                    visModel.Add(objVis);

                }
                return visModel;
            }

        }
        public List<ope_itens_producao_situacoes> get_ope_items_producao_situacoes(int id)
        {
            List<ope_itens_producao_situacoes> visModel = new List<ope_itens_producao_situacoes>();
            string str_query = "SELECT oips.id, ope_itens_id, data, descricao " +
                                    "FROM ope_itens_producao_situacoes oips " +
                                    " JOIN ope_itens oi ON oi.id = oips.ope_itens_id " +
                                    " JOIN producao_situacoes ps ON ps.id = oips.producao_situacoes_id " +
                                    " WHERE ope_itens_id = @id ";
            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int nId = Convert.ToInt32(reader["id"]);
                    int ope_itens_id = Convert.ToInt32(reader["ope_itens_id"]);

                    string data = reader["data"].ToString();
                    int ativo = Convert.ToInt32(reader["id"]);

                    ope_itens_producao_situacoes objVis = new ope_itens_producao_situacoes();
                    objVis.id = nId;
                    objVis.ope_itens_id = ope_itens_id;

                    objVis.data = data;
                    objVis.ativo = ativo;
                    visModel.Add(objVis);

                }
                return visModel;
            }
        }
        public List<opeVis> getopeVis(int id)
        {
            List<opeVis> visModel = new List<opeVis>();
            string str_query = "SELECT oi.id, si.codigo_smk, quantidade, valor_unitario, valor_total, oi.memo, si.descricao, numero_processo, es.nome " +
             "FROM  ope_itens oi JOIN smk_itens si ON si.id = oi.smk_itens_id " +
              " JOIN opes op ON op.id = oi.opes_id JOIN escolas es ON es.id = op.escolas_id  LEFT JOIN gastos ga ON ga.ope_itens_id = oi.id " +
              " WHERE op.id = '@id'";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int nId = Convert.ToInt32(reader["id"]);
                    string codigo_smk = reader["codigo_smk"].ToString();
                    string quantidade = reader["codigo_smk"].ToString();
                    string valor_unitario = reader["valor_unitario"].ToString();
                    string valor_total = reader["valor_total"].ToString();
                    string memo = reader["memo"].ToString();
                    string descricao = reader["descricao"].ToString();
                    string numero_processo = reader["numero_processo"].ToString();
                    string nome = reader["nome"].ToString();
                    int total = Convert.ToInt32(reader["id"]);

                    opeVis objVis = new opeVis();
                    objVis.id = nId;
                    objVis.codigo_smk = codigo_smk;

                    objVis.valor_unitario = valor_unitario;
                    objVis.memo = memo;
                    objVis.descricao = descricao;
                    objVis.nome = nome;
                    objVis.total = total;
                    visModel.Add(objVis);

                }

                conn.Close();
            }

            return visModel;

        }
        //
        // GET: /Ope/Create
        public ViewResult DetailsItemVis(int id)
        {
            opeItemDetails arrModel = new opeItemDetails();
            string str_query = @"SELECT oi.id, quantidade, valor_unitario, si.codigo_smk, valor_total, memo, descricao, numero_processo, es.nome, previsao_embarque
									FROM ope_itens oi
									JOIN smk_itens si ON si.id = oi.smk_itens_id
									JOIN opes op ON op.id = oi.opes_id
									JOIN escolas es ON es.id = op.escolas_id
									WHERE oi.id = @id";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int nId = Convert.ToInt32(reader["id"]);
                    int quantidade = Convert.ToInt32(reader["quantidade"]);
                    string codigo_smk = reader["codigo_smk"].ToString();
                    double valor_unitario = Convert.ToDouble(reader["valor_unitario"]);
                    string descricao = reader["descricao"].ToString();
                    string memo = reader["memo"].ToString();
                    string numero_processo = reader["numero_processo"].ToString();
                    string nome = reader["nome"].ToString();
                    string previsao_embarque = reader["previsao_embarque"].ToString();
                    double valor_total = Convert.ToDouble(reader["valor_total"]);

                    arrModel.id = nId;
                    arrModel.quantidade = quantidade;
                    arrModel.item = codigo_smk + "-" + descricao;
                    arrModel.memo = memo;
                    arrModel.valor_unitario = valor_unitario;
                    arrModel.nome = nome;

                    arrModel.valor_total = valor_total;
                    List<ope_itens_producao_situacoes_descricao> ope_itens_producao_situacoes = new List<ope_itens_producao_situacoes_descricao>();
                    ope_itens_producao_situacoes = getItemDetailsOpeItem(nId);
                    arrModel.ope_itens = ope_itens_producao_situacoes;
                    List<gastosItemDetail> gastosItemDetail = new List<gastosItemDetail>();
                    gastosItemDetail = getItemDetailGastos(nId);
                    arrModel.gastos = gastosItemDetail;

                }

                conn.Close();
            }

            return View(arrModel);


        }
        public List<ope_itens_producao_situacoes_descricao> getItemDetailsOpeItem(int id)
        {
            List<ope_itens_producao_situacoes_descricao> ope_itens_producao_situacoes = new List<ope_itens_producao_situacoes_descricao>();
            ope_itens_producao_situacoes_descricao objModel = new ope_itens_producao_situacoes_descricao();
            string str_query = @"SELECT oips.id, ope_itens_id, data, descricao
									FROM ope_itens_producao_situacoes oips
									JOIN ope_itens oi ON oi.id = oips.ope_itens_id
									JOIN producao_situacoes ps ON ps.id = oips.producao_situacoes_id
									WHERE ope_itens_id =@id";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int nId = Convert.ToInt32(reader["id"]);
                    int ope_itens_id = Convert.ToInt32(reader["ope_itens_id"]);
                    string data = reader["data"].ToString();
                    string descricao = reader["descricao"].ToString();
                    objModel.id = nId;
                    objModel.ope_itens_id = ope_itens_id;
                    objModel.data = data;
                    objModel.descricao = descricao;
                    ope_itens_producao_situacoes.Add(objModel);

                }

                conn.Close();
            }


            return ope_itens_producao_situacoes;
        }
        public List<gastosItemDetail> getItemDetailGastos(int id)
        {
            List<gastosItemDetail> gastos = new List<gastosItemDetail>();
            gastosItemDetail gastosobj = new gastosItemDetail();
            string str_query = @"select ga.id, ga.total, fo.nome, data, numero_documento from gastos ga
										join fornecedores fo on fo.id = ga.fornecedores_id where ga.ope_itens_id = @id";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int nId = Convert.ToInt32(reader["id"]);
                    double total = Convert.ToDouble(reader["total"]);
                    string data = reader["data"].ToString();
                    string numero_documento = reader["numero_documento"].ToString();
                    string nome = reader["nome"].ToString();

                    gastos.Add(gastosobj);
                }

                conn.Close();
            }

            return gastos;
        }



        public ActionResult Create()
        {
            return View();
        }

        public ActionResult OpeItemEdit(int id, string cod, int idvisualiza) 
        
        {
            opesEdit arrModel = new opesEdit();
            List<ope> opes = new List<ope>();

            string str_query = @"SELECT oi.id, quantidade, valor_unitario, valor_total, memo, smk_itens_id, si.codigo_smk, si.descricao, numero_processo, opes_id, ps.descricao AS ps_descricao, ps.id AS ps_id, previsao_embarque
		                            FROM ope_itens_producao_situacoes oips
		                               JOIN producao_situacoes ps ON ps.id = oips.producao_situacoes_id
		                               JOIN ope_itens oi ON oi.id = oips.ope_itens_id
		                               JOIN smk_itens si ON si.id = oi.smk_itens_id
		                               JOIN opes op ON op.id = oi.opes_id
		                            WHERE oi.id =@id ORDER BY oips.id DESC ";


            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int nid = Convert.ToInt32(reader["id"]);
               
                    string codigo_smk = reader["codigo_smk"].ToString();
                    string memo = reader["memo"].ToString();
                    int quantidade = Convert.ToInt32(reader["quantidade"]);
                    double valor_unitario =  Convert.ToDouble(reader["valor_unitario"]);
                    double valor_total =  Convert.ToDouble(reader["valor_total"]);

                    opes = db.Opes.ToList();
                    arrModel.opeList = opes;
                    arrModel.codigo_smk = codigo_smk;
                    arrModel.memo = memo;
                    arrModel.quantidade = quantidade;
                    arrModel.valor_unitario = valor_unitario;
                    arrModel.valor_total = valor_total;
                    arrModel.idvisualiza = idvisualiza;
                 
                }

                conn.Close();
            }

            return View(arrModel);

        }

        //
        public ActionResult buscaItem(string cod_smk, string op, string idvisualiza) {

           
            string op_String = op ;
            string cod_smkString = cod_smk ;
            string idvisualiza_String = idvisualiza ;
            string result = "";
           

            switch (op_String)
            {
                case "item_smk":

                    string str_query = @"SELECT * FROM smk_itens where codigo_smk like @cod_smk";

                    using (SqlConnection conn = new SqlConnection(str_connection))
                    using (SqlCommand cmd = new SqlCommand(str_query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cod_smk", cod_smkString);

                        conn.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                           
                            string codigo_smk = reader["codigo_smk"].ToString();
                            string descricao = reader["descricao"].ToString();
                            result = codigo_smk + "-" + descricao ;
                            
                        }
                        
                        conn.Close();
                    }   
                    
                    break;
                 
                 default:
                    
                    break;
            }

            return Json(new { ok = result });
            
        }
        // POST: /Ope/Create

        [HttpPost]
        public ActionResult Create(ope ope)
        {
            if (ModelState.IsValid)
            {
                db.Opes.Add(ope);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ope);
        }

        //
        // GET: /Ope/Edit/5

        public ActionResult Edit(int id)
        {

            opeEdit opeEdit = new opeEdit();
            List<escolas> escolas = new List<escolas>();

            string str_query = @"select op.id, numero_processo, observacao, es.nome, escolas_id, data_abertura, data_chegada from opes op join escolas es on es.id=op.escolas_id where op.id=@id";
            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int nId = Convert.ToInt32(reader["id"]);
                    string numero_processo = reader["numero_processo"].ToString();
                    string observacao = reader["observacao"].ToString();
                    string nome = reader["nome"].ToString();
                    int nid = Convert.ToInt32(reader["escolas_id"]);
                    string data_abertura = reader["data_abertura"].ToString();
                    string data_chegada = reader["data_chegada"].ToString();

                    opeEdit.id = nId;
                    opeEdit.escolas_id = nid;
                    opeEdit.numero_processo = numero_processo;
                    opeEdit.observacao = observacao;
                    opeEdit.data_abertura = data_abertura;
                    opeEdit.data_chegada = data_chegada;
                    escolas = db.escolas.ToList();
                    opeEdit.escolas = escolas;

                }

                conn.Close();
            }
            return View(opeEdit);
        }

        public ActionResult EditVis(int id)
        {
            ope_itens ope_items = db.Ope_itens.Find(id);
            return View(ope_items);
        }
        //
        // POST: /Ope/Edit/5
        [HttpPost]
        public ActionResult EditVis(ope_itens ope_itens)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ope_itens).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ope_itens);
        }
        [HttpPost]
        public ActionResult Edit(ope ope)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ope).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ope);
        }
         
        [HttpPost]
        public ActionResult opeItemEdit(ope_itens Ope_itens) 
        {
           
            
            return RedirectToAction("Index");
        }

        //
        // GET: /Ope/Delete/5

        public ActionResult Delete(int id)
        {
            ope ope = db.Opes.Find(id);
            return View(ope);
        }


        public ActionResult DeleteOpe(int id)
        {
            int i = 1;
            ope ope = db.Opes.Find(id);


            return View(ope);
        }


        //
        // POST: /Ope/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {

            ope ope = db.Opes.Find(id);
            db.Opes.Remove(ope);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    
public  string str_query { get; set; }}
}