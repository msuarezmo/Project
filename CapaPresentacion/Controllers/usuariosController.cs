using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CapaPresentacion.Models;

namespace CapaPresentacion.Controllers
{
    public class usuariosController : Controller
    {
        private colegioEntities db = new colegioEntities();

        // GET: usuarios
        public ActionResult Index()
        {
            var usuarios = db.usuarios.Include(u => u.curso).Include(u => u.estados).Include(u => u.novedades).Include(u => u.tipo_documento);
            return View(usuarios.ToList());
        }

        // GET: usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuarios usuarios = db.usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // GET: usuarios/Create
        public ActionResult Create()
        {
            ViewBag.id_curso = new SelectList(db.curso, "id", "jornada");
            ViewBag.id_estados = new SelectList(db.estados, "id", "descripcion");
            ViewBag.id = new SelectList(db.novedades, "id", "tipo");
            ViewBag.id_documento = new SelectList(db.tipo_documento, "id", "tipo");
            return View();
        }

        // POST: usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombres,apellidos,celular,numero_fijo,clave,documento,id_estados,correo,id_documento,id_curso")] usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                db.usuarios.Add(usuarios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_curso = new SelectList(db.curso, "id", "jornada", usuarios.id_curso);
            ViewBag.id_estados = new SelectList(db.estados, "id", "descripcion", usuarios.id_estados);
            ViewBag.id = new SelectList(db.novedades, "id", "tipo", usuarios.id);
            ViewBag.id_documento = new SelectList(db.tipo_documento, "id", "tipo", usuarios.id_documento);
            return View(usuarios);
        }

        // GET: usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuarios usuarios = db.usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_curso = new SelectList(db.curso, "id", "jornada", usuarios.id_curso);
            ViewBag.id_estados = new SelectList(db.estados, "id", "descripcion", usuarios.id_estados);
            ViewBag.id = new SelectList(db.novedades, "id", "tipo", usuarios.id);
            ViewBag.id_documento = new SelectList(db.tipo_documento, "id", "tipo", usuarios.id_documento);
            return View(usuarios);
        }

        // POST: usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombres,apellidos,celular,numero_fijo,clave,documento,id_estados,correo,id_documento,id_curso")] usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_curso = new SelectList(db.curso, "id", "jornada", usuarios.id_curso);
            ViewBag.id_estados = new SelectList(db.estados, "id", "descripcion", usuarios.id_estados);
            ViewBag.id = new SelectList(db.novedades, "id", "tipo", usuarios.id);
            ViewBag.id_documento = new SelectList(db.tipo_documento, "id", "tipo", usuarios.id_documento);
            return View(usuarios);
        }

        // GET: usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuarios usuarios = db.usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // POST: usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            usuarios usuarios = db.usuarios.Find(id);
            db.usuarios.Remove(usuarios);
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
