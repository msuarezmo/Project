using CapaDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Validations
{
    public class ValidationsLack : Datamodel
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
                return false;
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
    }
}
