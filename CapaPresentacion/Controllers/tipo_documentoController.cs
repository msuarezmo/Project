using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CapaPresentacion.Models;

namespace CapaPresentacion.Controllers
{
    public class tipo_documentoController : Controller
    {
        private colegioEntities db = new colegioEntities();

        // GET: tipo_documento
        public ActionResult Index()
        {
            return View(db.tipo_documento.ToList());
        }

        // GET: tipo_documento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_documento tipo_documento = db.tipo_documento.Find(id);
            if (tipo_documento == null)
            {
                return HttpNotFound();
            }
            return View(tipo_documento);
        }

        // GET: tipo_documento/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tipo_documento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,tipo,descripcion")] tipo_documento tipo_documento)
        {
            if (ModelState.IsValid)
            {
                db.tipo_documento.Add(tipo_documento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipo_documento);
        }

        // GET: tipo_documento/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_documento tipo_documento = db.tipo_documento.Find(id);
            if (tipo_documento == null)
            {
                return HttpNotFound();
            }
            return View(tipo_documento);
        }

        // POST: tipo_documento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,tipo,descripcion")] tipo_documento tipo_documento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipo_documento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipo_documento);
        }

        // GET: tipo_documento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_documento tipo_documento = db.tipo_documento.Find(id);
            if (tipo_documento == null)
            {
                return HttpNotFound();
            }
            return View(tipo_documento);
        }

        // POST: tipo_documento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tipo_documento tipo_documento = db.tipo_documento.Find(id);
            db.tipo_documento.Remove(tipo_documento);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
