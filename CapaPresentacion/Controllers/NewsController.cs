using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CapaDatos;
using CapaNegocio;
using System.Collections;

namespace CapaPresentacion.Controllers
{
    public class NewsController : Controller
    {
        private colegioEntities db = new colegioEntities();
        private SendEmail em = new SendEmail();
        
        [HttpGet]
        public ActionResult Index()
        {
            return View(db.News.ToList());
        }

        public ActionResult ConfirmacionEmail()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SendEmail(News news)
        {
            try
            {
                var queries = db.AspNetUsers.Where(x => x.AspNetRoles.All(z => z.Name == "Acudiente"));
                var description = db.News.Where(x => x.Id == news.Id);
                var date = db.News.Where(x => x.Id == news.Id);
               
                foreach (var val in queries)
                {
                    if (news.Active==false)
                    {
                        View("Index");
                    }
                    else
                    {
                        em.SendNews(val.Email.ToString(), news.Date.ToString(), news.Description.ToString());
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error" + ex);
        
            }
            return View("ConfirmacionEmail");
        }

        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.AspNetUsers.Where(x => x.AspNetRoles.All(z => z.Name == "Docente")), "Id", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdNews,Description,Date,Active,Id")] News news)
        {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.News.Add(news);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    ViewBag.Id = new SelectList(db.AspNetUsers.Where(x => x.AspNetRoles.All(z => z.Name == "Docente")), "Id", "FullName");
                    return View(news);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al agregar noticia"+ ex);
                    return View();
                }
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                News news = db.News.Find(id);
                if (news == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Id = new SelectList(db.AspNetUsers.Where(x => x.AspNetRoles.All(z => z.Name == "Docente")), "Id", "FullName");
                return View(news);
            
            

        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdNews,Description,Date,Active,Id")] News news)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    db.Entry(news).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                ViewBag.Id = new SelectList(db.AspNetUsers.Where(x => x.AspNetRoles.All(z => z.Name == "Docente")), "Id", "FullName");
                return View(news);

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Error al editar noticia" + ex);
                return View();

            }

        }

        // GET: News/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = await db.News.FindAsync(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                News news = await db.News.FindAsync(id);
                db.News.Remove(news);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Error al eliminar la noticia" + ex);
                return View();

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
