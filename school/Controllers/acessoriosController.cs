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
    public class acessoriosController : Controller
    {
        private GlobalInfo db = new GlobalInfo();

        //
        // GET: /acessorios/

        public ViewResult Index()
        {
            return View(db.acessorios.ToList());
        }

        //
        // GET: /acessorios/Details/5

        public ViewResult Details(int id)
        {
            acessorios acessorios = db.acessorios.Find(id);
            return View(acessorios);
        }

        //
        // GET: /acessorios/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /acessorios/Create

        [HttpPost]
        public ActionResult Create(acessorios acessorios)
        {
            if (ModelState.IsValid)
            {
                db.acessorios.Add(acessorios);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(acessorios);
        }
        
        //
        // GET: /acessorios/Edit/5
 
        public ActionResult Edit(int id)
        {
            acessorios acessorios = db.acessorios.Find(id);
            return View(acessorios);
        }

        //
        // POST: /acessorios/Edit/5

        [HttpPost]
        public ActionResult Edit(acessorios acessorios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acessorios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(acessorios);
        }

        //
        // GET: /acessorios/Delete/5
 
        public ActionResult Delete(int id)
        {
            acessorios acessorios = db.acessorios.Find(id);
            return View(acessorios);
        }

        //
        // POST: /acessorios/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            acessorios acessorios = db.acessorios.Find(id);
            db.acessorios.Remove(acessorios);
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