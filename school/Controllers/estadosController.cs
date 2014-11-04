using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using school.Models;

namespace school.Controllers
{
    public class estadosController : Controller
    {
        private GlobalInfo db = new GlobalInfo();

        //
        // GET: /estados/

        public ActionResult Index()
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(db.estados.ToList());
        }

        //
        // GET: /estados/Details/5

        public ViewResult Details(int id)
        {
            estados estados = db.estados.Find(id);
            return View(estados);
        }

        //
        // GET: /estados/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /estados/Create

        [HttpPost]
        public ActionResult Create(estados estados)
        {
            if (ModelState.IsValid)
            {
                db.estados.Add(estados);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(estados);
        }

        //
        // GET: /estados/Edit/5

        public ActionResult Edit(int id)
        {
            estados estados = db.estados.Find(id);
            return View(estados);
        }

        //
        // POST: /estados/Edit/5

        [HttpPost]
        public ActionResult Edit(estados estados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(estados);
        }

        //
        // GET: /estados/Delete/5

        public ActionResult Delete(int id)
        {
            estados estados = db.estados.Find(id);
            return View(estados);
        }

        //
        // POST: /estados/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            estados estados = db.estados.Find(id);
            db.estados.Remove(estados);
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