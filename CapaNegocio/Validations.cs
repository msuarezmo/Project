using CapaDatos;
using System;
using System.Linq;

namespace CapaNegocio
{
    public class ValidationsCourse
    {
        /// <summary>
        /// Modelo de base de datos
        /// </summary>
        private colegioEntities db = new colegioEntities();
        /// <summary>
        ///  Valida si el Director de curso seleccionado ya se encuentra asociado a un curso existente
        /// </summary>
        /// <param name="courses"></param>
        /// <returns></returns>
        public bool? createCourse(Courses courses)
        {
            try
            {
                var query = db.Courses;
                int total = query.Where(x => x.IdTeacher == courses.IdTeacher).Count(); ;
                if (total > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        /// <summary>
        /// Valida si el director de curso se encuentra registrado en otro curso
        /// </summary>
        /// <param name="courses"></param>
        /// <returns></returns>
        public bool? editCourse(Courses courses)
        {
            try
            {
                var query = db.Courses;
                int total = query.Where(x => x.IdTeacher == courses.IdTeacher && x.IdCourse != courses.IdCourse).Count();
                if (total > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
}
