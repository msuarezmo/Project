using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CapaDatos;

namespace CapaPresentacion.Controllers
{
    public class LacksController : Controller
    {
        private colegioEntities db = new colegioEntities();

        // GET: Lacks
        public ActionResult Index()
        {
            var lack = db.Lack.Include(l => l.Justification).Include(l => l.Students).Include(l => l.Subjects);
            return View(lack.ToList());
        }

        // GET: Lacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lack lack = db.Lack.Find(id);
            if (lack == null)
            {
                return HttpNotFound();
            }
            return View(lack);
        }

        // GET: Lacks/Create
        public ActionResult Create()
        {
            ViewBag.JustificationId = new SelectList(db.Justification, "IdJustification", "IdLack");
            ViewBag.idStudent = new SelectList(db.Students, "IdStudent", "Names");
            ViewBag.IdSubject = new SelectList(db.Subjects, "IdSubjects", "name");
            return View();
        }

        // POST: Lacks/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdLack,Description,Date,IdSubject,JustificationId,idStudent")] Lack lack)
        {
            if (ModelState.IsValid)
            {
                db.Lack.Add(lack);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JustificationId = new SelectList(db.Justification, "IdJustification", "IdLack", lack.JustificationId);
            ViewBag.idStudent = new SelectList(db.Students, "IdStudent", "Names", lack.idStudent);
            ViewBag.IdSubject = new SelectList(db.Subjects, "IdSubjects", "name", lack.IdSubject);
            return View(lack);
        }

        // GET: Lacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lack lack = db.Lack.Find(id);
            if (lack == null)
            {
                return HttpNotFound();
            }
            ViewBag.JustificationId = new SelectList(db.Justification, "IdJustification", "IdLack", lack.JustificationId);
            ViewBag.idStudent = new SelectList(db.Students, "IdStudent", "Names", lack.idStudent);
            ViewBag.IdSubject = new SelectList(db.Subjects, "IdSubjects", "name", lack.IdSubject);
            return View(lack);
        }

        // POST: Lacks/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdLack,Description,Date,IdSubject,JustificationId,idStudent")] Lack lack)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lack).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JustificationId = new SelectList(db.Justification, "IdJustification", "IdLack", lack.JustificationId);
            ViewBag.idStudent = new SelectList(db.Students, "IdStudent", "Names", lack.idStudent);
            ViewBag.IdSubject = new SelectList(db.Subjects, "IdSubjects", "name", lack.IdSubject);
            return View(lack);
        }

        // GET: Lacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lack lack = db.Lack.Find(id);
            if (lack == null)
            {
                return HttpNotFound();
            }
            return View(lack);
        }

        // POST: Lacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lack lack = db.Lack.Find(id);
            db.Lack.Remove(lack);
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
