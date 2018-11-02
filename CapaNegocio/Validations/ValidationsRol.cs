using CapaDominio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.Validations
{
    public class ValidationsRol : Datamodel
    {
        /// <summary>
        /// Devuelve todos los roles
        /// </summary>
        /// <returns></returns>
        public List<AspNetRoles> GetAllRoles()
        {
            try
            {
                return db.AspNetRoles.OrderByDescending(x => x.Name).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
