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
    public class transportadorasController : Controller
    {
        private GlobalInfo db = new GlobalInfo();

        //
        // GET: /transportadoras/

        public ActionResult Index()
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(db.transportadoras.ToList());
        }

        //
        // GET: /transportadoras/Details/5

        public ViewResult Details(int id)
        {
            transportadoras transportadoras = db.transportadoras.Find(id);
            return View(transportadoras);
        }

        //
        // GET: /transportadoras/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /transportadoras/Create

        [HttpPost]
        public ActionResult Create(transportadoras transportadoras)
        {
            if (ModelState.IsValid)
            {
                db.transportadoras.Add(transportadoras);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(transportadoras);
        }

        //
        // GET: /transportadoras/Edit/5

        public ActionResult Edit(int id)
        {
            transportadoras transportadoras = db.transportadoras.Find(id);
            return View(transportadoras);
        }

        //
        // POST: /transportadoras/Edit/5

        [HttpPost]
        public ActionResult Edit(transportadoras transportadoras)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transportadoras).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transportadoras);
        }

        //
        // GET: /transportadoras/Delete/5

        public ActionResult Delete(int id)
        {
            transportadoras transportadoras = db.transportadoras.Find(id);
            return View(transportadoras);
        }

        //
        // POST: /transportadoras/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            transportadoras transportadoras = db.transportadoras.Find(id);
            db.transportadoras.Remove(transportadoras);
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