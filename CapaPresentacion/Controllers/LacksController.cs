using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CapaDominio;
using CapaNegocio;
using CapaNegocio.Validations;
using PagedList;

namespace CapaPresentacion.Controllers
{
    public class LacksController : Controller
    {
        private Dispose dispose = new Dispose();
        private ValidationsLacks validationsLacks = new ValidationsLacks();
        private ValidationsUsers validationUser = new ValidationsUsers();
        private ValidationsSubject validationsSubject = new ValidationsSubject();
        private ValidationStudents validationStudents = new ValidationStudents();
        private ValidationsCourse validationsCourse = new ValidationsCourse();
        // GET: Lacks
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, string idTeacher, int? idSubject, DateTime? fechaIni, DateTime? fechaFin)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var Fecha1 = DateTime.UtcNow.AddHours(-5).Date;
            ViewBag.message = Fecha1.ToString();
            var Fecha2 = Fecha1.AddDays(1);
            var date1 = Convert.ToDateTime(fechaIni).ToString("yyyy-MM-dd");
            var date2 = Convert.ToDateTime(fechaFin).ToString("yyyy-MM-dd");
            if (fechaIni == null)
            {
                ViewBag.FechaIni = Fecha1.ToString("yyyy-MM-dd");
                ViewBag.FechaFin = Fecha2.ToString("yyyy-MM-dd");
            }
            else
            {
                ViewBag.FechaIni = date1;
                ViewBag.FechaFin = date2;
            }
            var teachers = validationUser.GetAllTeachers().OrderBy(x => x.FullName);
            ViewBag.IdTeacher = new SelectList(teachers, "Id", "FullName");
            var subjects = validationsSubject.GetAllSubjects().OrderBy(x => x.name);
            ViewBag.IdSubject = new SelectList(subjects, "IdSubjects", "Name");
            var lacks = validationsLacks.GetAllLacks();
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                lacks = lacks.Where(s => s.Students.Names.Contains(searchString) || s.Students.Surnames.Contains(searchString)).ToList();
            }
            if (!String.IsNullOrEmpty(idTeacher))
            {
                lacks = lacks.Where(x => x.IdTeacher == idTeacher).ToList();
            }
            if (idSubject != null)
            {
                lacks = lacks.Where(x => x.IdSubject == idSubject).ToList();
            }
            if (fechaIni != null && fechaFin != null)
            {
                lacks = lacks.Where(x => x.Date >= fechaIni && x.Date <= fechaFin).ToList();
            }
            if (fechaIni == null && fechaFin == null)
            {
                lacks = lacks.Where(x => x.Date >= Fecha1 && x.Date <= Fecha2).ToList();
            }

            lacks = lacks.OrderByDescending(s => s.Date).ToList();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(lacks.ToPagedList(pageNumber, pageSize));
        }


        // GET: Lacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lacks lacks = validationsLacks.FindById(id);
            if (lacks == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdTeacher = new SelectList(validationUser.GetAllTeachers(), "Id", "Email", lacks.IdTeacher);
            ViewBag.IdCourse = new SelectList(validationsCourse.GetAllCourses(), "IdCourse", "Description", lacks.IdCourse);
            ViewBag.IdStudent = new SelectList(validationStudents.GetAllStudents(), "IdStudent", "Names", lacks.IdStudent);
            ViewBag.IdSubject = new SelectList(validationsSubject.GetAllSubjects(), "IdSubjects", "name", lacks.IdSubject);
            return PartialView(lacks);
        }

        // POST: Lacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdLack,Date,IdCourse,IdStudent,IdSubject,IdTeacher,IdJustification")] Lacks lacks)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(lacks).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }
            ViewBag.IdTeacher = new SelectList(validationUser.GetAllTeachers(), "Id", "Email", lacks.IdTeacher);
            ViewBag.IdCourse = new SelectList(validationsCourse.GetAllCourses(), "IdCourse", "Description", lacks.IdCourse);
            ViewBag.IdStudent = new SelectList(validationStudents.GetAllStudents(), "IdStudent", "Names", lacks.IdStudent);
            ViewBag.IdSubject = new SelectList(validationsSubject.GetAllSubjects(), "IdSubjects", "name", lacks.IdSubject);
            return View(lacks);
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
