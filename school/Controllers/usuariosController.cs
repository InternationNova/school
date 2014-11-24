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
    public class usuariosController : Controller
    {
        private GlobalInfo db = new GlobalInfo();

        //
        // GET: /usuarios/

        public ViewResult Index()
        {
            return View(db.usuarios.ToList());
        }

        //
        // GET: /usuarios/Details/5

        public ViewResult Details(int id)
        {
            usuarios usuarios = db.usuarios.Find(id);
            return View(usuarios);
        }

        //
        // GET: /usuarios/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /usuarios/Create

        [HttpPost]
        public ActionResult Create(usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                db.usuarios.Add(usuarios);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(usuarios);
        }
        
        //
        // GET: /usuarios/Edit/5
 
        public ActionResult Edit(int id)
        {
            usuarios usuarios = db.usuarios.Find(id);
            return View(usuarios);
        }

        //
        // POST: /usuarios/Edit/5

        [HttpPost]
        public ActionResult Edit(usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuarios);
        }

        //
        // GET: /usuarios/Delete/5
 
        public ActionResult Delete(int id)
        {
            usuarios usuarios = db.usuarios.Find(id);
            return View(usuarios);
        }

        //
        // POST: /usuarios/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            usuarios usuarios = db.usuarios.Find(id);
            db.usuarios.Remove(usuarios);
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