using CapaDominio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.Validations
{
    public class ValidationsUser : Datamodel
    {
        private const string RolDocente = "Docente";
        private const string RolAcudiente = "Acudiente";
        private const string RolAdministrador = "Administrador";
        /// <summary>
        /// devuelve los docentes
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
    }
}
