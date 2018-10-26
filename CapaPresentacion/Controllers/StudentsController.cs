using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CapaDatos;
using PagedList;
using PagedList.Mvc;
using CapaNegocio;


namespace CapaPresentacion.Controllers
{
    public class StudentsController : Controller
    {
        private colegioEntities db = new colegioEntities();
        private ValidationStudents ValidationStudents = new ValidationStudents();
        // GET: Students
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? courseId)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.CourseId = new SelectList(db.Courses, "IdCourse", "Description");

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var students = from s in db.Students
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Names.Contains(searchString) || s.Surnames.Contains(searchString));
            }
            if (courseId != null && courseId > 0)
            {
                students = students.Where(x => x.CourseId == courseId);
            }
            students = students.OrderBy(s => s.Names);
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));
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
            return PartialView();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdStudent,Names,Surnames,DocumentTypeId,CourseId,Document,ParentId,Assistance")] Students students)
        {
            try
            {
                students.Assistance = false;
                bool? validation = ValidationStudents.CreateStudents(students);
                switch (validation)
                {
                    case true:
                        if (ModelState.IsValid)
                        {
                            db.Students.Add(students);
                            db.SaveChanges();
                            return JavaScript("$('#StudentsModal').modal('hide');" +
                                "window.setTimeout(function(){window.location.reload()}, 1500);" +
                                "toastr.success('Estudiante Creado!');");
                        }
                        else
                        {
                            return JavaScript("$('#StudentsModal').modal('hide');" +
                                      "toastr.error('Error al crear estudiante');");
                        };
                    case false:
                        var consulta = from m in db.AspNetUsers where m.AspNetRoles.Any(r => r.Name == "Acudiente") select m;
                        ViewBag.ParentId = new SelectList(consulta, "Id", "FullName", students.ParentId);
                        ViewBag.CourseId = new SelectList(db.Courses, "IdCourse", "Description");
                        ViewBag.DocumentTypeId = new SelectList(db.DocumentType, "Id", "Name");
                        ModelState.AddModelError("Document", "Número de documento asignado a otro estudiante");
                        return PartialView(students);

                    case null:
                        return JavaScript("$('#StudentsModal').modal('hide');" +
                                     "toastr.error('Error al crear estudiante');");
                }
                return JavaScript("$('#StudentsModal').modal('hide');" +
                                     "toastr.error('Error al crear estudiante');");
            }
            catch (Exception)
            {
                return JavaScript("$('#StudentsModal').modal('hide');" +
                                         "toastr.error('Error al crear estudiante');");
            }
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
            var consulta = from m in db.AspNetUsers where m.AspNetRoles.Any(r => r.Name == "Acudiente") select m;
            ViewBag.ParentId = new SelectList(consulta, "Id", "FullName", students.ParentId);
            ViewBag.CourseId = new SelectList(db.Courses, "IdCourse", "Description", students.CourseId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentType, "Id", "Name", students.DocumentTypeId);
            return PartialView(students);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdStudent,Names,Surnames,DocumentTypeId,CourseId,Document,ParentId,Assistance")] Students students)
        {
            try
            {
                students.Assistance = false;
                bool? validation = ValidationStudents.EditStudent(students);
                switch (validation)
                {
                    case true:
                        if (ModelState.IsValid)
                        {
                            db.Entry(students).State = EntityState.Modified;
                            db.SaveChanges();
                            return JavaScript("$('#StudentsModal').modal('hide');" +
                                "window.setTimeout(function(){window.location.reload()}, 1500);" +
                                "toastr.success('Estudiante editado correctamente!');");
                        }
                        else
                        {
                            return JavaScript("$('#StudentsModal').modal('hide');" +
                                 "toastr.error('Error al editar el estudiante seleccionado');");
                        };
                    case false:
                        var consulta = from m in db.AspNetUsers where m.AspNetRoles.Any(r => r.Name == "Acudiente") select m;
                        ViewBag.ParentId = new SelectList(consulta, "Id", "FullName", students.ParentId);
                        ViewBag.CourseId = new SelectList(db.Courses, "IdCourse", "Description");
                        ViewBag.DocumentTypeId = new SelectList(db.DocumentType, "Id", "Name");
                        ModelState.AddModelError("Document", "Número de documento asignado a otro estudiante");
                        return PartialView(students);
                    case null:
                        return JavaScript("$('#StudentsModal').modal('hide');" +
                          "toastr.error('Error al editar el estudiante seleccionado');");
                }
                return JavaScript("$('#StudentsModal').modal('hide');" +
                        "toastr.error('Error al editar el estudiante seleccionado');");
            }
            catch (Exception ex)
            {
                return JavaScript("$('#StudentsModal').modal('hide');" +
                           "toastr.error('Error al editar el estudiante seleccionado');");
            }
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
            return PartialView(students);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Students students = db.Students.Find(id);
                db.Students.Remove(students);
                db.SaveChanges();
                return JavaScript("$('#StudentsModal').modal('hide');" +
                               "window.setTimeout(function(){window.location.reload()}, 1500);" +
                               "toastr.success('Estudiante eliminado correctamente!');");
            }
            catch (Exception ex)
            {
                return JavaScript("$('#StudentsModal').modal('hide');" +
                           "toastr.error('No puede eliminar este estudiante');");
            }
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
