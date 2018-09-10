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
    public class novedadesController : Controller
    {
        private colegioEntities db = new colegioEntities();

        // GET: novedades
        public ActionResult Index()
        {
            var novedades = db.novedades.Include(n => n.usuarios);
            return View(novedades.ToList());
        }

        // GET: novedades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            novedades novedades = db.novedades.Find(id);
            if (novedades == null)
            {
                return HttpNotFound();
            }
            return View(novedades);
        }

        // GET: novedades/Create
        public ActionResult Create()
        {
            ViewBag.id = new SelectList(db.usuarios, "id", "nombres");
            return View();
        }

        // POST: novedades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,tipo,fecha_ingreso_de_novedad,fecha_solucion,respuesta,id_usuario")] novedades novedades)
        {
            if (ModelState.IsValid)
            {
                db.novedades.Add(novedades);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id = new SelectList(db.usuarios, "id", "nombres", novedades.id);
            return View(novedades);
        }

        // GET: novedades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            novedades novedades = db.novedades.Find(id);
            if (novedades == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = new SelectList(db.usuarios, "id", "nombres", novedades.id);
            return View(novedades);
        }

        // POST: novedades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,tipo,fecha_ingreso_de_novedad,fecha_solucion,respuesta,id_usuario")] novedades novedades)
        {
            if (ModelState.IsValid)
            {
                db.Entry(novedades).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id = new SelectList(db.usuarios, "id", "nombres", novedades.id);
            return View(novedades);
        }

        // GET: novedades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            novedades novedades = db.novedades.Find(id);
            if (novedades == null)
            {
                return HttpNotFound();
            }
            return View(novedades);
        }

        // POST: novedades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            novedades novedades = db.novedades.Find(id);
            db.novedades.Remove(novedades);
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
