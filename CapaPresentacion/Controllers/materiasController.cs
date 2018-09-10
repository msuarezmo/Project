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
    public class materiasController : Controller
    {
        private colegioEntities db = new colegioEntities();

        // GET: materias
        public ActionResult Index()
        {
            return View(db.materias.ToList());
        }

        // GET: materias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            materias materias = db.materias.Find(id);
            if (materias == null)
            {
                return HttpNotFound();
            }
            return View(materias);
        }

        // GET: materias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: materias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,descripcion,hora_inicio,hora_fin")] materias materias)
        {
            if (ModelState.IsValid)
            {
                db.materias.Add(materias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(materias);
        }

        // GET: materias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            materias materias = db.materias.Find(id);
            if (materias == null)
            {
                return HttpNotFound();
            }
            return View(materias);
        }

        // POST: materias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,descripcion,hora_inicio,hora_fin")] materias materias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(materias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(materias);
        }

        // GET: materias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            materias materias = db.materias.Find(id);
            if (materias == null)
            {
                return HttpNotFound();
            }
            return View(materias);
        }

        // POST: materias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            materias materias = db.materias.Find(id);
            db.materias.Remove(materias);
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
