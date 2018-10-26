using CapaDatos;
using System;
using System.Linq;

namespace CapaNegocio
{
    public class DataModel
    {
        /// <summary>
        /// Modelo de base de datos
        /// </summary>
        public colegioEntities db = new colegioEntities();
    }
    public class ValidationsCourse : DataModel
    {

        /// <summary>
        ///  Valida si el Director de curso seleccionado ya se encuentra asociado a un curso existente
        /// </summary>
        /// <param name="courses"></param>
        /// <returns></returns>
        public bool? CreateCourse(Courses courses)
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
                    return true;
                }

            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }

    public class ValidationSubjetcs : DataModel
    {/// <summary>
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
    public class ValidationStudents : DataModel
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
