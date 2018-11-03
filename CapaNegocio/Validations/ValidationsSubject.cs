using CapaDominio;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CapaNegocio
{
    public class ValidationsSubject : Datamodel
    {
        /// <summary>
        ///  Valida si el nombre la materia ya existe si no la inserta en bd
        /// </summary>
        /// <param name="subjects"></param>
        /// <returns></returns>
        public bool? CreateSubject(Subjects subjects)
        {
            try
            {
                var query = db.Subjects;
                int total = query.Where(x => x.name == subjects.name).Count();
                if (total > 0)
                {
                    return false;
                }
                else
                {
                    db.Subjects.Add(subjects);
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
        /// Valida si el nombre de la materia, se encuentra registrado en otra materia, si no actualiza el registro en bd
        /// </summary>
        /// <param name="subjects"></param>
        /// <returns></returns>
        public bool? EditSubject(Subjects subjects)
        {
            try
            {
                var query = db.Subjects;
                int total = query.Where(x => x.name == subjects.name && x.IdSubjects != subjects.IdSubjects).Count();
                if (total > 0)
                {
                    return false;
                }
                else
                {
                    db.Entry(subjects).State = EntityState.Modified;
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
        /// Devuelve todas las materias
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Subjects> GetAllSubjects()
        {
            try
            {
                return db.Subjects.OrderBy(x => x.name);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Busca una materia por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Subjects SearchById(int? id)
        {
            try
            {
                return db.Subjects.Find(id);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Elimina materia
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteStudent(int id)
        {
            try
            {
                Subjects subjects = SearchById(id);
                db.Subjects.Remove(subjects);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
