using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using school.Models;
using System.Configuration;

namespace school.Controllers
{
    public class categoriaSubProdutosController : Controller
    {
        private GlobalInfo db = new GlobalInfo();

        //
        // GET: /categoriaSubProdutos/

        public ActionResult Index()
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(db.categoria_sub_produtos.ToList());
        }

        //
        // GET: /categoriaSubProdutos/Details/5

        public ViewResult Details(int id)
        {
            categoria_sub_produtos categoria_sub_produtos = db.categoria_sub_produtos.Find(id);
            return View(categoria_sub_produtos);
        }

        //
        // GET: /categoriaSubProdutos/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /categoriaSubProdutos/Create

        [HttpPost]
        public ActionResult Create(categoria_sub_produtos categoria_sub_produtos)
        {
            if (ModelState.IsValid)
            {
                db.categoria_sub_produtos.Add(categoria_sub_produtos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoria_sub_produtos);
        }

        //
        // GET: /categoriaSubProdutos/Edit/5

        public ActionResult Edit(int id)
        {
            categoria_sub_produtos categoria_sub_produtos = db.categoria_sub_produtos.Find(id);
            return View(categoria_sub_produtos);
        }

        //
        // POST: /categoriaSubProdutos/Edit/5

        [HttpPost]
        public ActionResult Edit(categoria_sub_produtos categoria_sub_produtos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoria_sub_produtos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoria_sub_produtos);
        }

        //
        // GET: /categoriaSubProdutos/Delete/5

        public ActionResult Delete(int id)
        {
            categoria_sub_produtos categoria_sub_produtos = db.categoria_sub_produtos.Find(id);
            return View(categoria_sub_produtos);
        }

        //
        // POST: /categoriaSubProdutos/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            categoria_sub_produtos categoria_sub_produtos = db.categoria_sub_produtos.Find(id);
            db.categoria_sub_produtos.Remove(categoria_sub_produtos);
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