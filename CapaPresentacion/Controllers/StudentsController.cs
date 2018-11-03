using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using CapaNegocio;
using CapaDominio;
using CapaNegocio.Validations;

namespace CapaPresentacion.Controllers
{
    public class StudentsController : Controller
    {
        private ValidationStudents validationStudents = new ValidationStudents();
        private ValidationsCourse validationsCourse = new ValidationsCourse();
        private ValidationsUser validationsUser = new ValidationsUser();
        private ValidationsDocumenType validationsDocumenType = new ValidationsDocumenType();
        private Dispose dispose = new Dispose();
        // GET: Students
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? courseId)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var cursos = validationsCourse.GetAllCourses();
            ViewBag.CourseId = new SelectList(cursos, "IdCourse", "Description");

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var students = validationStudents.GetAllStudents();
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Names.Contains(searchString) || s.Surnames.Contains(searchString)).ToList();
            }
            if (courseId != null && courseId > 0)
            {
                students = students.Where(x => x.CourseId == courseId).ToList();
            }
            students = students.OrderBy(s => s.Names).ToList();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            var acudientes = validationsUser.GetAllParents();
            var cursos = validationsCourse.GetAllCourses();
            var documentos = validationsDocumenType.GetAllDocumentTypes();
            ViewBag.ParentId = new SelectList(acudientes, "Id", "FullName");
            ViewBag.CourseId = new SelectList(cursos, "IdCourse", "Description");
            ViewBag.DocumentTypeId = new SelectList(documentos, "Id", "Name");
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
                bool? validation = validationStudents.CreateStudents(students);
                switch (validation)
                {
                    case true:
                        return JavaScript("$('#StudentsModal').modal('hide');" +
                            "window.setTimeout(function(){window.location.reload()}, 1500);" +
                            "toastr.success('Estudiante Creado!');");
                    case false:
                        var acudientes = validationsUser.GetAllParents();
                        var cursos = validationsCourse.GetAllCourses();
                        var documentos = validationsDocumenType.GetAllDocumentTypes();
                        ViewBag.ParentId = new SelectList(acudientes, "Id", "FullName", students.ParentId);
                        ViewBag.CourseId = new SelectList(cursos, "IdCourse", "Description");
                        ViewBag.DocumentTypeId = new SelectList(documentos, "Id", "Name");
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
            Students students = validationStudents.SearchById(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            var acudientes = validationsUser.GetAllParents();
            var cursos = validationsCourse.GetAllCourses();
            var documentos = validationsDocumenType.GetAllDocumentTypes();
            ViewBag.ParentId = new SelectList(acudientes, "Id", "FullName", students.ParentId);
            ViewBag.CourseId = new SelectList(cursos, "IdCourse", "Description", students.CourseId);
            ViewBag.DocumentTypeId = new SelectList(documentos, "Id", "Name", students.DocumentTypeId);
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
                bool? validation = validationStudents.EditStudent(students);
                switch (validation)
                {
                    case true:
                        return JavaScript("$('#StudentsModal').modal('hide');" +
                            "window.setTimeout(function(){window.location.reload()}, 1500);" +
                            "toastr.success('Estudiante editado correctamente!');");
                    case false:
                        var acudientes = validationsUser.GetAllParents();
                        var cursos = validationsCourse.GetAllCourses();
                        var documentos = validationsDocumenType.GetAllDocumentTypes();
                        ViewBag.ParentId = new SelectList(acudientes, "Id", "FullName", students.ParentId);
                        ViewBag.CourseId = new SelectList(cursos, "IdCourse", "Description");
                        ViewBag.DocumentTypeId = new SelectList(documentos, "Id", "Name");
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
            Students students = validationStudents.SearchById(id);
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
                bool validation = validationStudents.DeleteStudent(id);
                if (validation)
                {
                    return JavaScript("$('#StudentsModal').modal('hide');" +
                               "window.setTimeout(function(){window.location.reload()}, 1500);" +
                               "toastr.success('Estudiante eliminado correctamente!');");
                }
                else
                {
                    return JavaScript("$('#StudentsModal').modal('hide');" +
                           "toastr.error('No puede eliminar este estudiante');");
                }

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
                dispose.Liberate();
            }
            base.Dispose(disposing);
        }
    }
}
