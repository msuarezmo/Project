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
using CapaNegocio.Validations;
using Microsoft.AspNet.Identity;

namespace CapaPresentacion.Controllers
{
    public class NewsController : Controller
    {
        private Dispose dispose = new Dispose();
        private ValidationsNews validationsNews = new ValidationsNews();
        private ValidationsCourse validationsCourse = new ValidationsCourse();
        private ValidationsUsers validationsUser = new ValidationsUsers();
        private ValidationStudents validationStudents = new ValidationStudents();

        // GET: News
        public ActionResult Index()
        {
            var user = validationsUser.SearchById(User.Identity.GetUserId());
            var users = validationsUser.GetAllAdmins();
            var news = validationsNews.GetAllNews();
            if (User.IsInRole("Docente"))
            {
                news = news.Where(x => x.CreatedBy == user.FullName || users.Any(z => z.FullName == x.CreatedBy));
                return View(news.ToList());
            }
            return View(news.ToList());
        }
        public ActionResult IndexParent()
        {
            var user = validationsUser.SearchById(User.Identity.GetUserId());
            var users = validationsUser.GetAllAdmins();
            var students = validationStudents.GetAllStudents();
            var news = validationsNews.GetAllNews();
            news = news.Where(x => students.Where(n => n.ParentId == user.Id).Any(z => z.CourseId == x.IdCourse) || users.Any(n => n.FullName == x.CreatedBy));
            return View(news.ToList());
        }


        // GET: News/Create
        public ActionResult Create()
        {
            var courses = validationsCourse.GetAllCourses();
            var user = User.Identity.GetUserId();
            courses = courses.Where(x => x.IdTeacher == user);
            ViewBag.IdCourse = new SelectList(courses, "IdCourse", "Description");
            return PartialView();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = validationsNews.SearchById(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return PartialView(news);
        }
        public ActionResult SendNew(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = validationsNews.SearchById(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return PartialView(news);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendNew(int id)
        {
            try
            {
                News news = validationsNews.SearchById(id);
                bool? send = validationsNews.SendNew(news);
                switch (send)
                {
                    case true:
                        return JavaScript("$('#NewsModal').modal('hide');" +
                            "window.setTimeout(function(){window.location.reload()}, 1500);" +
                            "toastr.success('Novedad Enviada!');");
                    case false:
                        return JavaScript("$('#NewsModal').modal('hide');" +
                            "window.setTimeout(function(){window.location.reload()}, 1500);" +
                            "toastr.warning('La cantidad de destinatarios es cero!');");

                    case null:
                        return JavaScript("$('#NewsModal').modal('hide');" +
                                     "toastr.error('Error al envíar Novedad');");
                }
                return JavaScript("$('#NewsModal').modal('hide');" +
                                   "toastr.error('Error al envíar Novedad');");
            }
            catch (Exception)
            {
                return JavaScript("$('#NewsModal').modal('hide');" +
                                                     "toastr.error('Error al envíar Novedad');");
            }

        }
        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdNews,Description,IdCourse,CreatedBy,LastSend")] News news)
        {
            try
            {
                var user = validationsUser.SearchById(User.Identity.GetUserId());
                news.CreatedBy = user.FullName;
                bool? validation = validationsNews.CreateNews(news);
                switch (validation)
                {
                    case true:
                        return JavaScript("$('#NewsModal').modal('hide');" +
                            "window.setTimeout(function(){window.location.reload()}, 1500);" +
                            "toastr.success('Novedad Creada!');");
                    case false:
                        var courses = validationsCourse.GetAllCourses();
                        courses = courses.Where(x => x.IdTeacher == user.Id);
                        ViewBag.IdCourse = new SelectList(courses, "IdCourse", "Description", news.IdCourse);
                        return PartialView(news);

                    case null:
                        return JavaScript("$('#NewsModal').modal('hide');" +
                                     "toastr.error('Error al Crear Novedad');");
                }
                return JavaScript("$('#NewsModal').modal('hide');" +
                                    "toastr.error('Error al Crear Novedad');");
            }
            catch (Exception)
            {
                return JavaScript("$('#NewsModal').modal('hide');" +
                                    "toastr.error('Error al Crear Novedad');");
            }
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = validationsNews.SearchById(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            var courses = validationsCourse.GetAllCourses();
            var user = User.Identity.GetUserId();
            courses = courses.Where(x => x.IdTeacher == user);
            ViewBag.IdCourse = new SelectList(courses, "IdCourse", "Description", news.IdCourse);
            return PartialView(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdNews,Description,IdCourse,CreatedBy,LastSend")] News news)
        {
            try
            {
                bool? validation = validationsNews.EditNews(news);
                switch (validation)
                {
                    case true:
                        return JavaScript("$('#NewsModal').modal('hide');" +
                            "window.setTimeout(function(){window.location.reload()}, 1500);" +
                            "toastr.success('Novedad editada correctamente!');");
                    case false:
                        var courses = validationsCourse.GetAllCourses();
                        var user = User.Identity.GetUserId();
                        courses = courses.Where(x => x.IdTeacher == user);
                        ViewBag.IdCourse = new SelectList(courses, "IdCourse", "Description", news.IdCourse);
                        return PartialView(news);
                    case null:
                        return JavaScript("$('#NewsModal').modal('hide');" +
                          "toastr.error('Error al editar el novedad seleccionada');");
                }
                return JavaScript("$('#NewsModal').modal('hide');" +
                        "toastr.error('Error al editar el novedad seleccionada');");
            }
            catch (Exception)
            {
                return JavaScript("$('#NewsModal').modal('hide');" +
                           "toastr.error('Error al editar el novedad seleccionada');");
            }
        }

        // GET: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = validationsNews.SearchById(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return PartialView(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                bool validation = validationsNews.DeleteNews(id);
                if (validation)
                {
                    return JavaScript("$('#NewsModal').modal('hide');" +
                               "window.setTimeout(function(){window.location.reload()}, 1500);" +
                               "toastr.success('Novedad eliminada correctamente!');");
                }
                else
                {
                    return JavaScript("$('#NewsModal').modal('hide');" +
                           "toastr.error('Error al eliminar novedad');");
                }
            }
            catch (Exception)
            {
                return JavaScript("$('#NewsModal').modal('hide');" +
                           "toastr.error('Error al eliminar novedad');");
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
