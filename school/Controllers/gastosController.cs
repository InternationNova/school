using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using school.Models;
using school.Classes;

namespace school.Controllers
{
    public class gastosController : Controller
    {
        private GlobalInfo db = new GlobalInfo();

        //
        // GET: /gastos/

        public ActionResult Index()
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(db.gastos.ToList());
        }

        //
        // GET: /gastos/Details/5

        public ViewResult Details(int id)
        {
            gastos gastos = db.gastos.Find(id);
            return View(gastos);
        }

        //
        // GET: /gastos/Create

        public ActionResult Create()
        {
            gastospost arrModel = new gastospost();
            List<fornecedores> fornecedores = new List<fornecedores>();
            fornecedores = db.fornecedores.ToList();
            List<tipo_documentos> tipo_documentos = new List<tipo_documentos>();
            tipo_documentos = db.tipo_documentos.ToList();


            arrModel.fornecedores = fornecedores;
            arrModel.tipodocumentos = tipo_documentos;

            return View(arrModel);
        }

        //
        // POST: /gastos/Create

        [HttpPost]
        public ActionResult Create(gastospost gastospost)
        {


            db.gastos.Add(gastospost.gastos);
            db.SaveChanges();
            return RedirectToAction("Index");


            return View(gastospost.gastos);
        }

        //
        // GET: /gastos/Edit/5

        public ActionResult Edit(int id)
        {
            gastos gastos = db.gastos.Find(id);
            return View(gastos);
        }

        //
        // POST: /gastos/Edit/5

        [HttpPost]
        public ActionResult Edit(gastos gastos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gastos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gastos);
        }

        //
        // GET: /gastos/Delete/5

        public ActionResult Delete(int id)
        {
            gastos gastos = db.gastos.Find(id);
            return View(gastos);
        }

        //
        // POST: /gastos/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            gastos gastos = db.gastos.Find(id);
            db.gastos.Remove(gastos);
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