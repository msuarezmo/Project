//-----------------------------------------------------------------------
// <copyright file="ValidationsUser.cs" company="COEF">
//    Todos los derechos reservados
// </copyright>
//-----------------------------------------------------------------------

namespace CapaNegocio.Validations
{
    using CapaDominio;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="ValidationsUser" />
    /// </summary>
    public class ValidationsUser : Datamodel
    {
        /// <summary>
        /// Defines the RolAcudiente
        /// </summary>
        private const string RolAcudiente = "Acudiente";

        /// <summary>
        /// Defines the RolAdministrador
        /// </summary>
        private const string RolAdministrador = "Administrador";

        /// <summary>
        /// Defines the RolDocente
        /// </summary>
        private const string RolDocente = "Docente";

        /// <summary>
        /// Elimina usuarios
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteUser(string id)
        {
            try
            {
                AspNetUsers users = SearchById(id);
                db.AspNetUsers.Remove(users);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Devuelve todos los administradores
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AspNetUsers> GetAllAdmins()
        {
            try
            {
                return db.AspNetUsers.Where(x => x.AspNetRoles.Any(r => r.Name == RolAdministrador));
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Devuelve todos los acudientes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AspNetUsers> GetAllParents()
        {
            try
            {
                return db.AspNetUsers.Where(x => x.AspNetRoles.Any(r => r.Name == RolAcudiente));
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Devuelve los docentes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AspNetUsers> GetAllTeachers()
        {
            try
            {
                return db.AspNetUsers.Where(x => x.AspNetRoles.Any(r => r.Name == RolDocente));
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Devuelve todos los usuarios
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AspNetUsers> GetAllUsers()
        {
            try
            {
                return db.AspNetUsers.OrderByDescending(x => x.FullName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Busca un usuario por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AspNetUsers SearchById(string id)
        {
            try
            {
                return db.AspNetUsers.Find(id);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Valida que el numero de documento y el tipo de documento no coindican con otro usuario, si no inserta en bd
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool? ValidateDocument(int? documentType, string document)
        {
            try
            {
                var query = db.AspNetUsers;
                int total = query.Where(x => x.Document == document && x.DocumentType == documentType).Count();
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
        public AspNetUsers SearchByEmail(string email)
        {
            try
            {
                return db.AspNetUsers.Where(x => x.Email == email).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }
         /// <summary>
        /// Valida que el correo ingresado, no se encuentre vinculado a otro usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool? ValidateEmail(string email)
        {
            try
            {
                var query = db.AspNetUsers;
                int total = query.Where(x => x.Email == email).Count();
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
        public bool SaveUser(AspNetUsers registerUser)
        {
            try
            {
                db.Entry(registerUser).State = EntityState.Modified;
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
