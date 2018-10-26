using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CapaDatos;
using CapaNegocio;
using CapaPresentacion.Models;
using PagedList;
using PagedList.Mvc;

namespace CapaPresentacion.Controllers
{
    public class CoursesController : Controller
    {
        private colegioEntities db = new colegioEntities();
        private ValidationsCourse validationsCourse = new ValidationsCourse();

        // GET: Courses
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, string IdTeacher)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.IdTeacher = new SelectList(db.AspNetUsers.Where(x => x.AspNetRoles.All(z => z.Name == "Docente")), "Id", "FullName");

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var courses = from s in db.Courses
                          select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Description.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(IdTeacher))
            {
                courses = courses.Where(x => x.IdTeacher == IdTeacher);
            }

            courses = courses.OrderBy(s => s.Description);
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(courses.ToPagedList(pageNumber, pageSize));
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Courses courses = db.Courses.Find(id);
            if (courses == null)
            {
                return HttpNotFound();
            }
            return View(courses);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            var consulta = from m in db.AspNetUsers where m.AspNetRoles.Any(r => r.Name == "Docente") select m;
            ViewBag.IdTeacher = new SelectList(consulta, "Id", "FullName");
            return PartialView();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCourse,Description,IdTeacher,TotalStudents")] Courses courses)
        {
            try
            {
                bool? validation = validationsCourse.CreateCourse(courses);
                switch (validation)
                {
                    case true:
                        if (ModelState.IsValid)
                        {
                            db.Courses.Add(courses);
                            db.SaveChanges();
                            return JavaScript("$('#CoursesModal').modal('hide');" +
                                "window.setTimeout(function(){window.location.reload()}, 1500);" +
                                "toastr.success('Curso Creado!');");
                        }
                        else
                        {
                            return JavaScript("$('#CoursesModal').modal('hide');" +
                                      "toastr.error('Error al Crear curso');");
                        };
                    case false:
                        var consulta = from m in db.AspNetUsers where m.AspNetRoles.Any(r => r.Name == "Docente") select m;
                        ViewBag.IdTeacher = new SelectList(consulta, "Id", "FullName", courses.IdTeacher);
                        ModelState.AddModelError("IdTeacher", "Director de curso se encuentra asignado");
                        return PartialView(courses);

                    case null:
                        return JavaScript("$('#CoursesModal').modal('hide');" +
                                     "toastr.error('Error al Crear curso');");
                }
                return JavaScript("$('#CoursesModal').modal('hide');" +
                                     "toastr.error('Error al Crear curso');");
            }
            catch (Exception)
            {
                return JavaScript("$('#CoursesModal').modal('hide');" +
                                         "toastr.error('Error al Crear curso');");
            }
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Courses courses = db.Courses.Find(id);
            if (courses == null)
            {
                return HttpNotFound();
            }
            var consulta = from m in db.AspNetUsers where m.AspNetRoles.Any(r => r.Name == "Docente") select m;
            ViewBag.IdTeacher = new SelectList(consulta, "Id", "FullName", courses.IdTeacher);
            return PartialView(courses);
        }
        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCourse,Description,IdTeacher,TotalStudents")] Courses courses)
        {
            try
            {
                bool? validation = validationsCourse.EditCourse(courses);
                switch (validation)
                {
                    case true:
                        if (ModelState.IsValid)
                        {
                            db.Entry(courses).State = EntityState.Modified;
                            db.SaveChanges();
                            return JavaScript("$('#CoursesModal').modal('hide');" +
                                "window.setTimeout(function(){window.location.reload()}, 1500);" +
                                "toastr.success('Curso editado correctamente!');");
                        }
                        else
                        {
                            return JavaScript("$('#CoursesModal').modal('hide');" +
                                 "toastr.error('Error al editar el curso seleccionado');");
                        };
                    case false:
                        var consulta = from m in db.AspNetUsers where m.AspNetRoles.Any(r => r.Name == "Docente") select m;
                        ViewBag.IdTeacher = new SelectList(consulta, "Id", "FullName", courses.IdTeacher);
                        ModelState.AddModelError("IdTeacher", "Director de curso se encuentra asignado");
                        return PartialView(courses);
                    case null:
                        return JavaScript("$('#CoursesModal').modal('hide');" +
                          "toastr.error('Error al editar el curso seleccionado');");
                }
                return JavaScript("$('#CoursesModal').modal('hide');" +
                        "toastr.error('Error al editar el curso seleccionado');");
            }
            catch (Exception ex)
            {
                return JavaScript("$('#CoursesModal').modal('hide');" +
                           "toastr.error('Error al editar el curso seleccionado');");
            }

        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Courses courses = db.Courses.Find(id);
            if (courses == null)
            {
                return HttpNotFound();
            }
            return PartialView(courses);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Courses courses = db.Courses.Find(id);
                db.Courses.Remove(courses);
                db.SaveChanges();
                return JavaScript("$('#CoursesModal').modal('hide');" +
                               "window.setTimeout(function(){window.location.reload()}, 1500);" +
                               "toastr.success('Curso eliminado correctamente!');");
            }
            catch (Exception ex)
            {
                return JavaScript("$('#CoursesModal').modal('hide');" +
                           "toastr.error('No puede eliminar este curso');");
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
