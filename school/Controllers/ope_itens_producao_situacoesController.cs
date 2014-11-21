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
    public class ope_itens_producao_situacoesController : Controller
    {
        private GlobalInfo db = new GlobalInfo();

        //
        // GET: /ope_itens_producao_situacoes/

        public ViewResult Index()
        {
            return View(db.ope_itens_producao_situacoes.ToList());
        }

        //
        // GET: /ope_itens_producao_situacoes/Details/5

        public ViewResult Details(int id)
        {
            ope_itens_producao_situacoes ope_itens_producao_situacoes = db.ope_itens_producao_situacoes.Find(id);
            return View(ope_itens_producao_situacoes);
        }

        //
        // GET: /ope_itens_producao_situacoes/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /ope_itens_producao_situacoes/Create

        [HttpPost]
        public ActionResult Create(ope_itens_producao_situacoes ope_itens_producao_situacoes)
        {
            if (ModelState.IsValid)
            {
                db.ope_itens_producao_situacoes.Add(ope_itens_producao_situacoes);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(ope_itens_producao_situacoes);
        }
        
        //
        // GET: /ope_itens_producao_situacoes/Edit/5
 
        public ActionResult Edit(int id)
        {
            ope_itens_producao_situacoes ope_itens_producao_situacoes = db.ope_itens_producao_situacoes.Find(id);
            return View(ope_itens_producao_situacoes);
        }

        //
        // POST: /ope_itens_producao_situacoes/Edit/5

        [HttpPost]
        public ActionResult Edit(ope_itens_producao_situacoes ope_itens_producao_situacoes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ope_itens_producao_situacoes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ope_itens_producao_situacoes);
        }

        //
        // GET: /ope_itens_producao_situacoes/Delete/5
 
        public ActionResult Delete(int id)
        {
            ope_itens_producao_situacoes ope_itens_producao_situacoes = db.ope_itens_producao_situacoes.Find(id);
            return View(ope_itens_producao_situacoes);
        }

        //
        // POST: /ope_itens_producao_situacoes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            ope_itens_producao_situacoes ope_itens_producao_situacoes = db.ope_itens_producao_situacoes.Find(id);
            db.ope_itens_producao_situacoes.Remove(ope_itens_producao_situacoes);
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