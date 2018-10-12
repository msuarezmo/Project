using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CapaDatos;
using CapaNegocio;
using PagedList;
using PagedList.Mvc;

namespace CapaPresentacion.Controllers
{
    public class SchedulesController : Controller
    {
        private colegioEntities db = new colegioEntities();
        private Validations Validations = new Validations();

        // GET: Schedules
        //public ActionResult Index()
        //{
        //    var schedule = db.Schedule.Include(s => s.AspNetUsers).Include(s => s.Courses).Include(s => s.Days).Include(s => s.Subjects);
        //    return View(schedule.ToList());
        //}
        public ActionResult Index(string IdTeacher, int? IdCourse, int? IdDay, int? IdSubject)
        {

            ViewBag.IdTeacher = new SelectList(db.AspNetUsers.Where(x => x.AspNetRoles.All(z => z.Name == "Docente")), "Id", "FullName");
            ViewBag.IdCourse = new SelectList(db.Courses, "IdCourse", "Description");
            ViewBag.IdDay = new SelectList(db.Days, "Id", "Name");
            ViewBag.IdSubject = new SelectList(db.Subjects, "IdSubjects", "name");

            var horarios = from s in db.Schedule
                           select s;

            if (IdTeacher != null && IdTeacher != "")
            {
                horarios = horarios.Where(x => x.IdTeacher == IdTeacher);
            }
            if (IdCourse != null)
            {
                horarios = horarios.Where(x => x.IdCourse == IdCourse);
            }
            if (IdDay != null)
            {
                horarios = horarios.Where(x => x.IdDay == IdDay);
            }
            if (IdSubject != null)
            {
                horarios = horarios.Where(x => x.IdSubject == IdSubject);
            }
            horarios = horarios.OrderBy(s => s.IdDay);
            int? page = 1;
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(horarios.ToPagedList(pageNumber, pageSize));
        }

        // GET: Schedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedule.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // GET: Schedules/Create
        public ActionResult Create()
        {
            ViewBag.FromHour = DateTime.UtcNow.AddHours(-5).ToString("HH:mm");
            ViewBag.ToHour = DateTime.UtcNow.AddHours(-4).ToString("HH:mm");
            ViewBag.IdTeacher = new SelectList(db.AspNetUsers.Where(x => x.AspNetRoles.All(z => z.Name == "Docente")), "Id", "FullName");
            ViewBag.IdCourse = new SelectList(db.Courses, "IdCourse", "Description");
            ViewBag.IdDay = new SelectList(db.Days, "Id", "Name");
            ViewBag.IdSubject = new SelectList(db.Subjects, "IdSubjects", "name");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdCourse,IdSubject,IdTeacher,IdDay,HourFrom,HourTo")] Schedule schedule)
        {
            int? test = Validations.ValidateSchedule(schedule);
            if (test == 0)
            {
                ModelState.AddModelError("IdDay", "El horario seleccionado para este día, ya esta ocupado");
                ViewBag.IdTeacher = new SelectList(db.AspNetUsers, "Id", "Email", schedule.IdTeacher);
                ViewBag.IdCourse = new SelectList(db.Courses, "IdCourse", "Description", schedule.IdCourse);
                ViewBag.IdDay = new SelectList(db.Days, "Id", "Name", schedule.IdDay);
                ViewBag.IdSubject = new SelectList(db.Subjects, "IdSubjects", "name", schedule.IdSubject);
                return View(schedule);
            }
            if (test == 1)
            {
                if (ModelState.IsValid)
                {
                    db.Schedule.Add(schedule);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.IdTeacher = new SelectList(db.AspNetUsers, "Id", "Email", schedule.IdTeacher);
                ViewBag.IdCourse = new SelectList(db.Courses, "IdCourse", "Description", schedule.IdCourse);
                ViewBag.IdDay = new SelectList(db.Days, "Id", "Name", schedule.IdDay);
                ViewBag.IdSubject = new SelectList(db.Subjects, "IdSubjects", "name", schedule.IdSubject);
                return View(schedule);
            }
            if (test == 2)
            {
                ModelState.AddModelError("IdTeacher", "El docente seleccionado, ya esta agregado en este horario");
                ViewBag.IdTeacher = new SelectList(db.AspNetUsers, "Id", "Email", schedule.IdTeacher);
                ViewBag.IdCourse = new SelectList(db.Courses, "IdCourse", "Description", schedule.IdCourse);
                ViewBag.IdDay = new SelectList(db.Days, "Id", "Name", schedule.IdDay);
                ViewBag.IdSubject = new SelectList(db.Subjects, "IdSubjects", "name", schedule.IdSubject);
                return View(schedule);
            }

            ViewBag.IdTeacher = new SelectList(db.AspNetUsers, "Id", "Email", schedule.IdTeacher);
            ViewBag.IdCourse = new SelectList(db.Courses, "IdCourse", "Description", schedule.IdCourse);
            ViewBag.IdDay = new SelectList(db.Days, "Id", "Name", schedule.IdDay);
            ViewBag.IdSubject = new SelectList(db.Subjects, "IdSubjects", "name", schedule.IdSubject);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedule.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdTeacher = new SelectList(db.AspNetUsers, "Id", "Email", schedule.IdTeacher);
            ViewBag.IdCourse = new SelectList(db.Courses, "IdCourse", "Description", schedule.IdCourse);
            ViewBag.IdDay = new SelectList(db.Days, "Id", "Name", schedule.IdDay);
            ViewBag.IdSubject = new SelectList(db.Subjects, "IdSubjects", "name", schedule.IdSubject);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdCourse,IdSubject,IdTeacher,IdDay,HourFrom,HourTo")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdTeacher = new SelectList(db.AspNetUsers, "Id", "Email", schedule.IdTeacher);
            ViewBag.IdCourse = new SelectList(db.Courses, "IdCourse", "Description", schedule.IdCourse);
            ViewBag.IdDay = new SelectList(db.Days, "Id", "Name", schedule.IdDay);
            ViewBag.IdSubject = new SelectList(db.Subjects, "IdSubjects", "name", schedule.IdSubject);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedule.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Schedule schedule = db.Schedule.Find(id);
            db.Schedule.Remove(schedule);
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
