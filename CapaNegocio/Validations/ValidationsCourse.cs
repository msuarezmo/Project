//-----------------------------------------------------------------------
// <copyright file="ValidationsCourse.cs" company="COEF">
//    Todos los derechos reservados
// </copyright>
//-----------------------------------------------------------------------

namespace CapaNegocio
{
    using CapaDominio;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="ValidationsCourse" />
    /// </summary>
    public class ValidationsCourse : Datamodel
    {
        /// <summary>
        /// Valida si el Director de curso seleccionado ya se encuentra asociado a un curso existente, si no lo inserta en bd
        /// </summary>
        /// <param name="courses"></param>
        /// <returns></returns>
        public bool? CreateCourse(Courses courses)
        {
            try
            {
                var query = db.Courses;
                int total = query.Where(x => x.IdTeacher == courses.IdTeacher).Count();
                if (total > 0)
                {
                    return false;
                }
                else
                {
                    db.Courses.Add(courses);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Elimina curso
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteCourse(int id)
        {
            try
            {
                Courses courses = SearchById(id);
                db.Courses.Remove(courses);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Valida si el director de curso se encuentra registrado en otro curso, si no actualiza el registro
        /// </summary>
        /// <param name="courses"></param>
        /// <returns></returns>
        public bool? EditCourse(Courses courses)
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
                    db.Entry(courses).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Devuelve todos los cursos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Courses> GetAllCourses()
        {
            try
            {
                return db.Courses.OrderByDescending(x => x.Description);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Busca un curso por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Courses SearchById(int? id)
        {
            try
            {
                return db.Courses.Find(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
