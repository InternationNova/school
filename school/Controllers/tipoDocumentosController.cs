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
    public class tipoDocumentosController : Controller
    {
        private GlobalInfo db = new GlobalInfo();

        //
        // GET: /tipoDocumentos/

        public ActionResult Index()
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(db.tipo_documentos.ToList());
        }

        //
        // GET: /tipoDocumentos/Details/5

        public ViewResult Details(int id)
        {
            tipo_documentos tipo_documentos = db.tipo_documentos.Find(id);
            return View(tipo_documentos);
        }

        //
        // GET: /tipoDocumentos/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /tipoDocumentos/Create

        [HttpPost]
        public ActionResult Create(tipo_documentos tipo_documentos)
        {
            if (ModelState.IsValid)
            {
                db.tipo_documentos.Add(tipo_documentos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipo_documentos);
        }

        //
        // GET: /tipoDocumentos/Edit/5

        public ActionResult Edit(int id)
        {
            tipo_documentos tipo_documentos = db.tipo_documentos.Find(id);
            return View(tipo_documentos);
        }

        //
        // POST: /tipoDocumentos/Edit/5

        [HttpPost]
        public ActionResult Edit(tipo_documentos tipo_documentos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipo_documentos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipo_documentos);
        }

        //
        // GET: /tipoDocumentos/Delete/5

        public ActionResult Delete(int id)
        {
            tipo_documentos tipo_documentos = db.tipo_documentos.Find(id);
            return View(tipo_documentos);
        }

        //
        // POST: /tipoDocumentos/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            tipo_documentos tipo_documentos = db.tipo_documentos.Find(id);
            db.tipo_documentos.Remove(tipo_documentos);
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