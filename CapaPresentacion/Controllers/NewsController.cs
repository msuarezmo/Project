using CapaDominio;
using CapaNegocio.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class NewsController : Controller
    {
        private ValidationsNews validationsNews = new ValidationsNews();
        private ValidationsUser validationUser = new ValidationsUser();

        [HttpGet]
        public ActionResult Index()
        {
            var listNews = validationsNews.listNews();
            return View(listNews);
        }


        // GET: Courses/Create
        public ActionResult Create()
        {
            var docentes = validationUser.GetAllTeachers();
            ViewBag.Id = new SelectList(docentes, "Id", "FullName");
            return PartialView();
        }


        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Description,Date,Active,Id")] News news)
        {
            try
            {
                bool? validation = validationsNews.createNews(news);
                switch (validation)
                {
                    case true:
                        return JavaScript("$('#NewsModal').modal('hide');" +
                            "window.setTimeout(function(){window.location.reload()}, 1500);" +
                            "toastr.success('Novedad Creada!');");
                    case false:
                        var docentes = validationUser.GetAllTeachers();
                        ViewBag.IdTeacher = new SelectList(docentes, "IdTeacher", "FullName", news.Id);
                        ModelState.AddModelError("IdTeacher", "Director de curso se encuentra asignado");
                        return PartialView(news);

                    case null:
                        return JavaScript("$('#NewsModal').modal('hide');" +
                                     "toastr.error('Error al crear la novedad');");
                }
                return JavaScript("$('#NewsModal').modal('hide');" +
                                     "toastr.error('Error al crear la novedad');");


            }
            catch (Exception ex)
            {
                return View();
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

        // POST: Courses/Delete/5
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
                           "toastr.error('No puede eliminar este curso');");
                }

            }
            catch (Exception ex)
            {
                return JavaScript("$('#NewsModal').modal('hide');" +
                           "toastr.error('No puede eliminar este curso');");
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
            var docentes = validationUser.GetAllTeachers();
            ViewBag.IdTeacher = new SelectList(docentes, "Id", "FullName", news.Id);
            return PartialView(news);
        }
        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Description,Date,Active")] News news)
        {
            try
            {
                    validationsNews.EditNews(news);
                                    return JavaScript("$('#NewsModal').modal('hide');" +
                    "window.setTimeout(function(){window.location.reload()}, 1500);" +
                    "toastr.success('Novedad editada correctamente!');");
                
            }
            catch (Exception ex)
            {
                return JavaScript("$('#NewsModal').modal('hide');" +
                           "toastr.error('Error al editar la novedad seleccionada');");
            }

        }


    }


}