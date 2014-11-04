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
    public class sub_produtosController : Controller
    {
        private GlobalInfo db = new GlobalInfo();

        //
        // GET: /sub_produtos/

        public ViewResult Index()
        {
            return View(db.sub_produtos.ToList());
        }

        //
        // GET: /sub_produtos/Details/5

        public ViewResult Details(int id)
        {
            sub_produtos sub_produtos = db.sub_produtos.Find(id);
            return View(sub_produtos);
        }

        //
        // GET: /sub_produtos/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /sub_produtos/Create

        [HttpPost]
        public ActionResult Create(sub_produtos sub_produtos)
        {
            if (ModelState.IsValid)
            {
                db.sub_produtos.Add(sub_produtos);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(sub_produtos);
        }
        
        //
        // GET: /sub_produtos/Edit/5
 
        public ActionResult Edit(int id)
        {
            sub_produtos sub_produtos = db.sub_produtos.Find(id);
            return View(sub_produtos);
        }

        //
        // POST: /sub_produtos/Edit/5

        [HttpPost]
        public ActionResult Edit(sub_produtos sub_produtos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sub_produtos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sub_produtos);
        }

        //
        // GET: /sub_produtos/Delete/5
 
        public ActionResult Delete(int id)
        {
            sub_produtos sub_produtos = db.sub_produtos.Find(id);
            return View(sub_produtos);
        }

        //
        // POST: /sub_produtos/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            sub_produtos sub_produtos = db.sub_produtos.Find(id);
            db.sub_produtos.Remove(sub_produtos);
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