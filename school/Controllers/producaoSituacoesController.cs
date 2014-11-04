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
    public class producaoSituacoesController : Controller
    {
        private GlobalInfo db = new GlobalInfo();

        //
        // GET: /producaoSituacoes/

        public ActionResult Index()
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(db.producao_situacoes.ToList());
        }

        //
        // GET: /producaoSituacoes/Details/5

        public ViewResult Details(int id)
        {
            producao_situacoes producao_situacoes = db.producao_situacoes.Find(id);
            return View(producao_situacoes);
        }

        //
        // GET: /producaoSituacoes/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /producaoSituacoes/Create

        [HttpPost]
        public ActionResult Create(producao_situacoes producao_situacoes)
        {
            if (ModelState.IsValid)
            {
                db.producao_situacoes.Add(producao_situacoes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producao_situacoes);
        }

        //
        // GET: /producaoSituacoes/Edit/5

        public ActionResult Edit(int id)
        {
            producao_situacoes producao_situacoes = db.producao_situacoes.Find(id);
            return View(producao_situacoes);
        }

        //
        // POST: /producaoSituacoes/Edit/5

        [HttpPost]
        public ActionResult Edit(producao_situacoes producao_situacoes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producao_situacoes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producao_situacoes);
        }

        //
        // GET: /producaoSituacoes/Delete/5

        public ActionResult Delete(int id)
        {
            producao_situacoes producao_situacoes = db.producao_situacoes.Find(id);
            return View(producao_situacoes);
        }

        //
        // POST: /producaoSituacoes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            producao_situacoes producao_situacoes = db.producao_situacoes.Find(id);
            db.producao_situacoes.Remove(producao_situacoes);
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