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
    public class perdaController : Controller
    {
        private GlobalInfo db = new GlobalInfo();
        string str_connection = ConfigurationManager.ConnectionStrings["GlobalInfo"].ConnectionString;
        //
        // GET: /perda/

        public ActionResult Index()
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<perdaIndex> perdaList = new List<perdaIndex>();

            string str_query = @"select  t1.* , t2.descricao from perdas as t1,  materia_primas as t2 
                                 where  t1.materia_primas_id = t2.id";
            
            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
               
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    perdaIndex perdaIndexObj = new perdaIndex();
                    perda perda = new perda();

                    int nId = Convert.ToInt32(reader["id"]);

                    int quantidade = Convert.ToInt32(reader["quantidade"]);
                    string descricao = reader["descricao"].ToString();
                    string unidade = reader["unidade"].ToString();

                    perda.id = nId;
                    perda.quantidade = quantidade;
                    perda.unidade = unidade;
                    perdaIndexObj.decriaco = descricao;
                    perdaIndexObj.perda = perda;

                    perdaList.Add(perdaIndexObj);

                    perdaIndexObj = null;
                    perda = null;

                }

                conn.Close();
            }

            return View(perdaList);

        }

        //
        // GET: /perda/Details/5

        public ViewResult Details(int id)
        {
            perda perda = db.perdas.Find(id);
            return View(perda);
        }

        //
        // GET: /perda/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /perda/Create

        [HttpPost]
        public ActionResult Create(perda perda)
        {
            if (ModelState.IsValid)
            {
                db.perdas.Add(perda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(perda);
        }

        //
        // GET: /perda/Edit/5

        public ActionResult Edit(int id)
        {
            perda perda = db.perdas.Find(id);

            perdaEdit perdaEdit = new perdaEdit();
            List<materia_primas> materia_primas = new List<materia_primas>();



            string str_query = @"select co.id, mp.id as mpid, descricao, quantidade, co.unidade from perdas co
											join materia_primas mp on mp.id = co.materia_primas_id
											where co.id = @id";
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
                    string descricao = reader["descricao"].ToString();
                    string unidade = reader["unidade"].ToString();

                    perdaEdit.quantidade = quantidade;
                    perdaEdit.unidade = unidade;
                    materia_primas = db.materia_primas.ToList();

                    perdaEdit.materia_primas = materia_primas;

                }

                conn.Close();
            }
            return View(perdaEdit);
        }

        //
        // POST: /perda/Edit/5

        [HttpPost]
        public ActionResult Edit(perda perda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(perda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(perda);
        }

        //
        // GET: /perda/Delete/5

        public ActionResult Delete(int id)
        {
            perda perda = db.perdas.Find(id);
            return View(perda);
        }

        //
        // POST: /perda/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            perda perda = db.perdas.Find(id);
            db.perdas.Remove(perda);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}