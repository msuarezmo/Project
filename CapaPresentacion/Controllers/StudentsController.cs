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
    public class StudentsController : Controller
    {
        private colegioEntities db = new colegioEntities();

        // GET: Students
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.AspNetUsers).Include(s => s.Courses).Include(s => s.DocumentType);
            return View(students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students students = db.Students.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            var consulta = from m in db.AspNetUsers where m.AspNetRoles.Any(r => r.Name == "Acudiente") select m;
            ViewBag.ParentId = new SelectList(consulta, "Id", "FullName");
            ViewBag.CourseId = new SelectList(db.Courses, "IdCourse", "Description");
            ViewBag.DocumentTypeId = new SelectList(db.DocumentType, "Id", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdStudent,Names,Surnames,DocumentTypeId,CourseId,Document,ParentId")] Students students)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(students);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParentId = new SelectList(db.AspNetUsers, "Id", "Email", students.ParentId);
            ViewBag.CourseId = new SelectList(db.Courses, "IdCourse", "Description", students.CourseId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentType, "Id", "Name", students.DocumentTypeId);
            return View(students);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students students = db.Students.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(db.AspNetUsers, "Id", "Email", students.ParentId);
            ViewBag.CourseId = new SelectList(db.Courses, "IdCourse", "Description", students.CourseId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentType, "Id", "Name", students.DocumentTypeId);
            return View(students);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdStudent,Names,Surnames,DocumentTypeId,CourseId,Document,ParentId")] Students students)
        {
            if (ModelState.IsValid)
            {
                db.Entry(students).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentId = new SelectList(db.AspNetUsers, "Id", "Email", students.ParentId);
            ViewBag.CourseId = new SelectList(db.Courses, "IdCourse", "Description", students.CourseId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentType, "Id", "Name", students.DocumentTypeId);
            return View(students);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students students = db.Students.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Students students = db.Students.Find(id);
            db.Students.Remove(students);
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
