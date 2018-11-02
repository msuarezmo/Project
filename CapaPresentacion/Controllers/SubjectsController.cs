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
    public class SubjectsController : Controller
    {
        private ValidationsSubject validationSubjetcs = new ValidationsSubject();
        private Dispose dispose = new Dispose();

        // GET: Subjects
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var subjects = validationSubjetcs.GetAllSubjects();
            if (!String.IsNullOrEmpty(searchString))
            {
                subjects = subjects.Where(s => s.name.Contains(searchString)).ToList();
            }

            subjects = subjects.OrderBy(s => s.name).ToList();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(subjects.ToPagedList(pageNumber, pageSize));
        }

        // GET: Subjects/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdSubjects,name")] Subjects subjects)
        {
            try
            {
                bool? validation = validationSubjetcs.CreateSubject(subjects);
                switch (validation)
                {
                    case true:

                        return JavaScript("$('#SubjectModal').modal('hide');" +
                            "window.setTimeout(function(){window.location.reload()}, 1500);" +
                            "toastr.success('Materia creada!');");
                    case false:
                        ModelState.AddModelError("name", "Ya existe una materia con este nombre");
                        return PartialView();

                    case null:
                        return JavaScript("$('#SubjectModal').modal('hide');" +
                                     "toastr.error('Error al crear materia');");
                }
                return JavaScript("$('#SubjectModal').modal('hide');" +
                                     "toastr.error('Error al crear materia');");
            }
            catch (Exception)
            {
                return JavaScript("$('#SubjectModal').modal('hide');" +
                                         "toastr.error('Error al crear materia');");
            }
        }

        // GET: Subjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subjects subjects = validationSubjetcs.SearchById(id);
            if (subjects == null)
            {
                return HttpNotFound();
            }
            return PartialView(subjects);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSubjects,name")] Subjects subjects)
        {
            try
            {
                bool? validation = validationSubjetcs.EditSubject(subjects);
                switch (validation)
                {
                    case true:
                        return JavaScript("$('#SubjectModal').modal('hide');" +
                            "window.setTimeout(function(){window.location.reload()}, 1500);" +
                            "toastr.success('materia editada correctamente!');");
                    case false:
                        ModelState.AddModelError("name", "Ya existe una materia con este nombre");
                        return PartialView();
                    case null:
                        return JavaScript("$('#SubjectModal').modal('hide');" +
                          "toastr.error('Error al editar la  materia');");
                }
                return JavaScript("$('#SubjectModal').modal('hide');" +
                        "toastr.error('Error al editar materia selecionada');");
            }
            catch (Exception ex)
            {
                return JavaScript("$('#SubjectModal').modal('hide');" +
                           "toastr.error('Error al editar materia selecionada');");
            }
        }

        // GET: Subjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subjects subjects = validationSubjetcs.SearchById(id);
            if (subjects == null)
            {
                return HttpNotFound();
            }
            return PartialView(subjects);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                bool validation = validationSubjetcs.DeleteStudent(id);
                if (validation)
                {
                    return JavaScript("$('#SubjectModal').modal('hide');" +
                                   "window.setTimeout(function(){window.location.reload()}, 1500);" +
                                   "toastr.success('Materia eliminada correctamente!');");
                }
                else
                {
                    return JavaScript("$('#SubjectModal').modal('hide');" +
                           "toastr.error('No puede eliminar esta materia');");
                }

            }
            catch (Exception ex)
            {
                return JavaScript("$('#SubjectModal').modal('hide');" +
                           "toastr.error('No puede eliminar esta materia');");
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
