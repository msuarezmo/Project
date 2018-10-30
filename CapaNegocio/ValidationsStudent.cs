using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class ValidationStudents : Datamodel
    {
        /// <summary>
        /// Valida que el numero de documento y el tipo de documento no coindican con otro estudiante
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
                    return true;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Valida que el numero de documento y el tipo de documento no coindican con otro estudiante, excepto si es el mismo id
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
