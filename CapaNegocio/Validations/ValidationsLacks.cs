using CapaDominio;
using System;
using System.Collections.Generic;


namespace CapaNegocio.Validations
{
    public class ValidationsLacks : Datamodel
    {
        public IEnumerable<Lacks> GetAllLacks()
        {
            try
            {
                return db.Lacks;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool SaveLacks(List<Students> fails, int idCourse, int idSubject, DateTime today, string user)
        {
            try
            {
                foreach (Students student in fails)
                {
                    Lacks lack = new Lacks
                    {
                        IdCourse = idCourse,
                        IdTeacher = user,
                        IdSubject = idSubject,
                        Date = today,
                        IdStudent = student.IdStudent
                    };
                    SaveLack(lack);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void SaveLack(Lacks lack)
        {
            try
            {
                db.Lacks.Add(lack);
                db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Lacks FindById(int? id)
        {
            try
            {
                return db.Lacks.Find(id);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
