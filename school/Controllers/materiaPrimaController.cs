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
    public class materiaPrimaController : Controller
    {
        private GlobalInfo db = new GlobalInfo();
        string str_connection = ConfigurationManager.ConnectionStrings["GlobalInfo"].ConnectionString;
        
        //
        // GET: /materiaPrima/

        public ActionResult Index()
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<materiaPrimaIndex> materiaPrimaList = new List<materiaPrimaIndex>();

            string str_query = @"select  t1.* , t2.descricao AS description from materia_primas as t1,  categoria_sub_produtos as t2 
                                 where  t1.categoria_materia_primas_id = t2.id";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    materiaPrimaIndex materiaPrimaObj = new materiaPrimaIndex();
                    materia_primas materiaPrima = new materia_primas();

                    int nId = Convert.ToInt32(reader["id"]);

                   
                    string codigo_smk = reader["codigo_smk"].ToString();
                    string descricao = reader["descricao"].ToString();
                    string unidade = reader["unidade"].ToString();
                    string description = reader["description"].ToString();

                    materiaPrima.id = nId;
                    materiaPrima.codigo_smk = codigo_smk;
                    materiaPrima.descricao = descricao;
                    materiaPrima.unidade = unidade;

                    materiaPrimaObj.materia_primas = materiaPrima;
                    materiaPrimaObj.decriaco = description;

                    materiaPrimaList.Add(materiaPrimaObj);
                
                }

                conn.Close();
            }

            return View(materiaPrimaList);

        }

        //
        // GET: /materiaPrima/Details/5

        public ViewResult Details(int id)
        {
            materia_primas materia_primas = db.materia_primas.Find(id);
            return View(materia_primas);
        }

        //
        // GET: /materiaPrima/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /materiaPrima/Create

        [HttpPost]
        public ActionResult Create(materia_primas materia_primas)
        {
            if (ModelState.IsValid)
            {
                db.materia_primas.Add(materia_primas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(materia_primas);
        }

        //
        // GET: /materiaPrima/Edit/5

        public ActionResult Edit(int id)
        {
            materiaPrimaEdit materiaPrimaEdit = new materiaPrimaEdit();

            List<categoria_materia_primas> categoria_materia_primas = new List<categoria_materia_primas>();
            materia_primas materia_primas = new materia_primas();
            materia_primas = db.materia_primas.Find(id);

            categoria_materia_primas = db.categoria_materia_primas.ToList();
            materiaPrimaEdit.categoria_materia_primas = categoria_materia_primas;
            materiaPrimaEdit.id = materia_primas.id;
            materiaPrimaEdit.codigo_smk = materia_primas.codigo_smk;
            materiaPrimaEdit.descricao = materia_primas.descricao;
            materiaPrimaEdit.unidade = materia_primas.unidade;
            materiaPrimaEdit.categoria_materia_primas_id = materia_primas.categoria_materia_primas_id;


            return View(materiaPrimaEdit);
        }

        //
        // POST: /materiaPrima/Edit/5

        [HttpPost]
        public ActionResult Edit(materia_primas materia_primas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(materia_primas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(materia_primas);
        }

        //
        // GET: /materiaPrima/Delete/5

        public ActionResult Delete(int id)
        {
            materia_primas materia_primas = db.materia_primas.Find(id);
            return View(materia_primas);
        }

        //
        // POST: /materiaPrima/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            materia_primas materia_primas = db.materia_primas.Find(id);
            db.materia_primas.Remove(materia_primas);
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