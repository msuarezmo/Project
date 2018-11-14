using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CapaDominio;
using CapaNegocio.Validations;
using PagedList;
using PagedList.Mvc;

namespace CapaPresentacion.Controllers
{
    public class UsersController : Controller
    {
        private ValidationsUsers validationsUser = new ValidationsUsers();
        private ValidationsRol validationsRol = new ValidationsRol();
        private ValidationsDocumenType validationsDocumenType = new ValidationsDocumenType();
        private Dispose dispose = new Dispose();


        // GET: AspNetUsers
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, string rol)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var roles = validationsRol.GetAllRoles();
            ViewBag.Rol = new SelectList(roles, "Id", "Name");
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            var users = validationsUser.GetAllUsers();
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.FullName.Contains(searchString)).ToList();
            }
            if (!String.IsNullOrEmpty(rol))
            {
                users = users.Where(x => x.AspNetRoles.All(z => z.Id == rol)).ToList(); ;
            }
            users = users.OrderBy(s => s.FullName).ToList();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));
        }

        // GET: AspNetUsers/Create
        //public ActionResult Create()
        //{
        //    ViewBag.DocumentType = new SelectList(db.DocumentType, "Id", "Name");
        //    return View();
        //}

        //// POST: AspNetUsers/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,FirstName,SecondName,Surname,SecondSurname,Document,DocumentType")] AspNetUsers aspNetUsers)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.AspNetUsers.Add(aspNetUsers);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.DocumentType = new SelectList(db.DocumentType, "Id", "Name", aspNetUsers.DocumentType);
        //    return View(aspNetUsers);
        //}

        // GET: AspNetUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = validationsUser.SearchById(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            var documentos = validationsDocumenType.GetAllDocumentTypes();
            ViewBag.DocumentType = new SelectList(documentos, "Id", "Name", aspNetUsers.DocumentType);
            return PartialView(aspNetUsers);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,FirstName,SecondName,Surname,SecondSurname,Document,DocumentType")] AspNetUsers aspNetUsers)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(aspNetUsers).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.DocumentType = new SelectList(db.DocumentType, "Id", "Name", aspNetUsers.DocumentType);
            return View(aspNetUsers);
        }

        // GET: AspNetUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = validationsUser.SearchById(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return PartialView(aspNetUsers);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            bool validation = validationsUser.DeleteUser(id);
            if (validation)
            {
                return JavaScript("$('#UsersModal').modal('hide');" +
                           "window.setTimeout(function(){window.location.reload()}, 1500);" +
                           "toastr.success('Usuario eliminado correctamente!');");
            }
            else
            {
                return JavaScript("$('#UsersModal').modal('hide');" +
                       "toastr.error('Error al eliminar usuarios');");
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
