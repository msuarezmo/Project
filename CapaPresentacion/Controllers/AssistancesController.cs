using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CapaDominio;
using CapaNegocio;
using CapaNegocio.Email;
using CapaNegocio.Validations;
using Microsoft.AspNet.Identity;
using PagedList;
namespace CapaPresentacion.Controllers
{
    public class AssistancesController : Controller
    {
        private ValidationsAssistance validationsAssistance = new ValidationsAssistance();
        private ValidationsSubject validationsSubject = new ValidationsSubject();
        private ValidationStudents validationStudents = new ValidationStudents();
        private ValidationsCourse validationsCourse = new ValidationsCourse();
        private ValidationsLack validationsLack = new ValidationsLack();
        private ValidationsUser validationUser = new ValidationsUser();
        private Dispose dispose = new Dispose();
        private SendEmail sendEmail = new SendEmail();

        // GET: Assistances
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, string idTeacher, int? idSubject, DateTime? fechaIni, DateTime? fechaFin)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            if (fechaIni == null)
            {
                var Fecha1 = DateTime.Today;
                var Fecha2 = Fecha1.AddDays(1);
                ViewBag.FechaIni = DateTime.Today.ToString("yyyy-MM-dd");
                ViewBag.FechaFin = Fecha2.ToString("yyyy-MM-dd");
            }
            else
            {
                var date1 = Convert.ToDateTime(fechaIni).ToString("yyyy-MM-dd");
                var date2 = Convert.ToDateTime(fechaFin).ToString("yyyy-MM-dd");
                ViewBag.FechaIni = date1;
                ViewBag.FechaFin = date2;
            }
            var teachers = validationUser.GetAllTeachers().OrderBy(x => x.FullName);
            ViewBag.IdTeacher = new SelectList(teachers, "Id", "FullName");
            var subjects = validationsSubject.GetAllSubjects().OrderBy(x => x.name);
            ViewBag.IdSubject = new SelectList(subjects, "IdSubjects", "Name");
            var Asistencias = validationsAssistance.GetAllAsistences();
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
                Asistencias = Asistencias.Where(s => s.Students.Names.Contains(searchString) || s.Students.Surnames.Contains(searchString)).ToList();
            }
            if (!String.IsNullOrEmpty(idTeacher))
            {
                Asistencias = Asistencias.Where(x => x.IdTeacher == idTeacher).ToList();
            }
            if (idSubject != null)
            {
                Asistencias = Asistencias.Where(x => x.IdSubject == idSubject).ToList();
            }
            if (fechaIni != null && fechaFin != null)
            {
                Asistencias = Asistencias.Where(x => x.Date >= fechaIni && x.Date <= fechaFin).ToList();
            }

            Asistencias = Asistencias.OrderByDescending(s => s.Date).ToList();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(Asistencias.ToPagedList(pageNumber, pageSize));
        }

        // GET: Assistances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assistances assistances = validationsAssistance.SearchById(id);
            if (assistances == null)
            {
                return HttpNotFound();
            }
            return PartialView(assistances);
        }

        // GET: Assistances/Create
        public ActionResult Create()
        {
            var today = DateTime.UtcNow.AddHours(-5);
            ViewBag.Today = today.ToString("yyyy-MM-dd");
            var courses = validationsCourse.GetAllCourses();
            var teacher = validationUser.GetAllTeachers().Where(x => x.Id == User.Identity.GetUserId());
            var subjects = validationsSubject.GetAllSubjects();
            ViewBag.IdTeacher = new SelectList(teacher, "Id", "FullName");
            ViewBag.IdCourse = new SelectList(courses, "IdCourse", "Description");
            ViewBag.IdSubject = new SelectList(subjects, "IdSubjects", "name");
            return View();
        }
        public ActionResult Students(int idCourse)
        {
            var students = validationStudents.GetStudensByCourse(idCourse);
            return View(students);
        }
        public ActionResult SaveAssistances(List<int> ids, int idCourse, int idSubject)
        {
            if (ids == null || ids.Count == 0)
            {
                return JavaScript("toastr.warning('Debe seleccionar al menos un estudiante');");
            }
            else
            {
                string user = User.Identity.GetUserId();
                var today = DateTime.UtcNow.AddHours(-5);
                List<Assistances> assistances = new List<Assistances>();
                List<Students> fails = validationStudents.GetStudensByCourse(idCourse).ToList();
                List<Students> studentsInClass = validationStudents.GetStudensById(ids);
                fails = (from c in fails
                         where !(from o in studentsInClass
                                 select o.IdStudent)
                                .Contains(c.IdStudent)
                         select c).ToList();

                foreach (Students student in studentsInClass)
                {
                    Assistances assistance = new Assistances
                    {
                        IdTeacher = user,
                        IdCourse = idCourse,
                        IdSubject = idSubject,
                        Date = today,
                        IdStudent = student.IdStudent
                    };
                    assistances.Add(assistance);
                }
                bool saveAssistances = validationsAssistance.SaveAssistances(assistances);
                if (saveAssistances)
                {
                    if (fails.Count > 0)
                    {
                        bool saveLacks = validationsLack.SaveLacks(fails, idCourse, idSubject, today, user);
                        if (saveLacks)
                        {
                            foreach (var item in fails)
                            {
                                sendEmail.SendLack(item, user, idSubject);
                            }
                            return JavaScript("toastr.success('Fallas y asistencias registradas');" +
                                "window.setTimeout(function(){window.location.href = '/Assistances/Index'}, 1500);");
                        }
                        else
                        {
                            return JavaScript("toastr.error('Error al registrar fallas');");
                        }
                    }
                    else
                    {
                        return JavaScript("toastr.success('No hay fallas que registrar');" +
                                "window.setTimeout(function(){window.location.href = '/Assistances/Index'}, 1500);");
                    }
                }
                else
                {
                    return JavaScript("toastr.error('Error al registrar Asistencias');");
                }
            }
        }
        // POST: Assistances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdAssistance,IdTeacher,Date,IdCourse,IdSubject,IdStudent")] Assistances assistances)
        {

            var today = DateTime.UtcNow.AddHours(-5);
            ViewBag.Today = today.ToString("yyyy-MM-dd");
            var courses = validationsCourse.GetAllCourses();
            var teacher = validationUser.GetAllTeachers().Where(x => x.Id == User.Identity.GetUserId());
            var subjects = validationsSubject.GetAllSubjects();
            ViewBag.IdTeacher = new SelectList(teacher, "Id", "FullName");
            ViewBag.IdCourse = new SelectList(courses, "IdCourse", "Description");
            ViewBag.IdSubject = new SelectList(subjects, "IdSubjects", "name");
            return View();
        }

        // GET: Assistances/Edit/5
        public ActionResult Edit(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Assistances assistances = validationsAssistance.SearchById(id);
            //if (assistances == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.IdTeacher = new SelectList(db.AspNetUsers, "Id", "Email", assistances.IdTeacher);
            //ViewBag.IdCourse = new SelectList(db.Courses, "IdCourse", "Description", assistances.IdCourse);
            //ViewBag.IdSubject = new SelectList(db.Subjects, "IdSubjects", "name", assistances.IdSubject);
            return View(assistances);
        }

        // POST: Assistances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdAssistance,IdTeacher,Date,IdCourse,IdSubject")] Assistances assistances)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(assistances).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.IdTeacher = new SelectList(db.AspNetUsers, "Id", "Email", assistances.IdTeacher);
            //ViewBag.IdCourse = new SelectList(db.Courses, "IdCourse", "Description", assistances.IdCourse);
            //ViewBag.IdSubject = new SelectList(db.Subjects, "IdSubjects", "name", assistances.IdSubject);
            return View(assistances);
        }

        // GET: Assistances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assistances assistances = validationsAssistance.SearchById(id);
            if (assistances == null)
            {
                return HttpNotFound();
            }
            return View(assistances);
        }

        // POST: Assistances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Assistances assistances = db.Assistances.Find(id);
            //db.Assistances.Remove(assistances);
            //db.SaveChanges();
            return RedirectToAction("Index");
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
