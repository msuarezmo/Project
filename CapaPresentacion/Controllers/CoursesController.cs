using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CapaDominio;
using CapaNegocio;
using CapaNegocio.Email;
using CapaNegocio.Validations;
using CapaPresentacion.Models;
using PagedList;
using PagedList.Mvc;

namespace CapaPresentacion.Controllers
{
    public class CoursesController : Controller
    {
        private ValidationsCourse validationsCourse = new ValidationsCourse();
        private ValidationsUser validationUser = new ValidationsUser();
        private Dispose dispose = new Dispose();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, string IdTeacher)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var acudientes = validationUser.GetAllParents();
            ViewBag.IdTeacher = new SelectList(acudientes, "Id", "FullName");

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            var courses = validationsCourse.GetAllCourses();
            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Description.Contains(searchString)).ToList();
            }
            if (!String.IsNullOrEmpty(IdTeacher))
            {
                courses = courses.Where(x => x.IdTeacher == IdTeacher).ToList(); ;
            }
            courses = courses.OrderBy(s => s.Description).ToList(); ;
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(courses.ToPagedList(pageNumber, pageSize));
        }
        // GET: Courses/Create
        public ActionResult Create()
        {
            var acudientes = validationUser.GetAllParents();
            ViewBag.IdTeacher = new SelectList(acudientes, "Id", "FullName");
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
                        return JavaScript("$('#CoursesModal').modal('hide');" +
                            "window.setTimeout(function(){window.location.reload()}, 1500);" +
                            "toastr.success('Curso Creado!');");
                    case false:
                        var acudientes = validationUser.GetAllParents();
                        ViewBag.IdTeacher = new SelectList(acudientes, "Id", "FullName", courses.IdTeacher);
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
            Courses courses = validationsCourse.SearchById(id);
            if (courses == null)
            {
                return HttpNotFound();
            }
            var acudientes = validationUser.GetAllTeachers();
            ViewBag.IdTeacher = new SelectList(acudientes, "Id", "FullName", courses.IdTeacher);
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

                        return JavaScript("$('#CoursesModal').modal('hide');" +
                            "window.setTimeout(function(){window.location.reload()}, 1500);" +
                            "toastr.success('Curso editado correctamente!');");

                    case false:
                        var acudientes = validationUser.GetAllParents();
                        ViewBag.IdTeacher = new SelectList(acudientes, "Id", "FullName", courses.IdTeacher);
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
            Courses courses = validationsCourse.SearchById(id);
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
                bool validation = validationsCourse.DeleteCourse(id);
                if (validation)
                {
                    return JavaScript("$('#CoursesModal').modal('hide');" +
                               "window.setTimeout(function(){window.location.reload()}, 1500);" +
                               "toastr.success('Curso eliminado correctamente!');");
                }
                else
                {
                    return JavaScript("$('#CoursesModal').modal('hide');" +
                           "toastr.error('Existen estudiantes asociados a este curso');");
                }
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
                dispose.Liberate();
            }
            base.Dispose(disposing);
        }
    }
}
