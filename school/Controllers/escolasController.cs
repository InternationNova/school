using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using school.Models;
using school.Classes;
using System.Data.SqlClient;
using System.Configuration;

namespace school.Controllers
{
    public class escolasController : Controller
    {
        private GlobalInfo db = new GlobalInfo();
        string str_connection = ConfigurationManager.ConnectionStrings["GlobalInfo"].ConnectionString;

        //
        // GET: /escolas/

        public ActionResult Index()
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<escolasIndex> escolasIndex = new List<escolasIndex>();

            string str_query = @"select * from escolas";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    escolasIndex arrescolaIndex = new escolasIndex();
                    escolas escolas = new escolas();

                    int nId = Convert.ToInt32(reader["id"]);
                    string nome = reader["nome"].ToString();
                    string endereco = reader["endereco"].ToString();
                    string bairro = reader["bairro"].ToString();
                    string cidade = reader["cidade"].ToString();
                    string cep = reader["cep"].ToString();
                    string telefone = reader["telefone"].ToString();
                    int estados_id = Convert.ToInt32(reader["estados_id"]);

                    escolas.id = nId;
                    escolas.nome = nome;
                    escolas.endereco = endereco;
                    escolas.bairro = bairro;
                    escolas.cidade = cidade;
                    escolas.cep = cep;
                    escolas.telefone = telefone;
                    escolas.estados_id = estados_id;

                    arrescolaIndex.escolas = escolas;

                    string estadosNome = getEstadosNome(estados_id);
                    arrescolaIndex.estados_nome = estadosNome;
                    escolasIndex.Add(arrescolaIndex);
                    arrescolaIndex = null;
                    escolas = null;

                }

                conn.Close();
            }

            return View(escolasIndex);
        }

        public string getEstadosNome(int id)
        {
            string estadosNome = "";

            estados estados = new estados();

            estados = db.estados.Find(id);
            estadosNome = estados.nome;

            return estadosNome;
        }
        //
        // GET: /escolas/Details/5

        public ViewResult Details(int id)
        {
            escolas escolas = db.escolas.Find(id);
            estados estados = db.estados.Find(escolas.estados_id);
            escolasDetail escoalsDetails = new escolasDetail();
            escoalsDetails.escolas = escolas;
            escoalsDetails.estadosName = estados.nome;


            return View(escoalsDetails);
        }

        //
        // GET: /escolas/Create

        public ActionResult Create()
        {
            escolasCreate escolasCreateObj = new escolasCreate();
            List<estados> estadosArr = new List<estados>();

            estadosArr = db.estados.ToList();

            escolasCreateObj.estadosArr = estadosArr;

            return View(escolasCreateObj);
            
            
            return View();
        }

        //
        // POST: /escolas/Create

        [HttpPost]
        public ActionResult Create(escolas escolas)
        {
            if (ModelState.IsValid)
            {
                db.escolas.Add(escolas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(escolas);
        }

        //
        // GET: /escolas/Edit/5

        public ActionResult Edit(int id)
        {
            escolasEdit arrModel = new escolasEdit();

            escolas escolas = db.escolas.Find(id);
            List<estados> estados = db.estados.ToList();
            arrModel.escolas = escolas;
            arrModel.estados = estados;
            return View(arrModel);
        }

        //
        // POST: /escolas/Edit/5

        [HttpPost]
        public ActionResult Edit(escolas escolas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(escolas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(escolas);
        }

        //
        // GET: /escolas/Delete/5

        public ActionResult Delete(int id)
        {
            escolas escolas = db.escolas.Find(id);
            estados estados = db.estados.Find(escolas.estados_id);
            escolasDetail escoalsDetails = new escolasDetail();
            escoalsDetails.escolas = escolas;
            escoalsDetails.estadosName = estados.nome;

            return View(escoalsDetails);
        }

        //
        // POST: /escolas/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            string str_query = "delete from escolas where id = @id";
            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();

                cmd.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();


                conn.Close();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}