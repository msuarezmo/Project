using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class ValidationsSubject : Datamodel
    {
        /// <summary>
        ///  Valida si el nombre la materia ya existe
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
                    return true;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Valida si el nombre de la materia, se encuentra registrado en otra materia
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
                    return true;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
