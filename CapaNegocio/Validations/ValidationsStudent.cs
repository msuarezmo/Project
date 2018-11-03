//-----------------------------------------------------------------------
// <copyright file="ValidationsStudent.cs" company="COEF">
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
    /// Defines the <see cref="ValidationStudents" />
    /// </summary>
    public class ValidationStudents : Datamodel
    {
        /// <summary>
        /// Valida que el numero de documento y el tipo de documento no coindican con otro estudiante, si no inserta en bd
        /// </summary>
        /// <param name="students"></param>
        /// <returns></returns>
        public bool? CreateStudents(Students students)
        {
            try
            {
                var query = db.Students;
                int total = query.Where(x => x.Document == students.Document && x.DocumentTypeId == students.DocumentTypeId).Count();
                if (total > 0)
                {
                    return false;
                }
                else
                {
                    db.Students.Add(students);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Elimina estudiante
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteStudent(int id)
        {
            try
            {
                Students students = SearchById(id);
                db.Students.Remove(students);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Valida que el numero de documento y el tipo de documento no coindican con otro estudiante, excepto si es el mismo id, si no actualiza el registro
        /// </summary>
        /// <param name="students"></param>
        /// <returns></returns>
        public bool? EditStudent(Students students)
        {
            try
            {
                var query = db.Students;
                int total = query.Where(x => x.Document == students.Document && x.DocumentTypeId == students.DocumentTypeId && x.IdStudent != students.IdStudent).Count();
                if (total > 0)
                {
                    return false;
                }
                else
                {
                    db.Entry(students).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Devuelve todos los estudiantes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Students> GetAllStudents()
        {
            try
            {
                return db.Students.OrderByDescending(x => x.Names);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Busca un estudiante por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Students SearchById(int? id)
        {
            try
            {
                return db.Students.Find(id);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
