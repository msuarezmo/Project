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
        public IEnumerable<AspNetRoles> GetAllRoles()
        {
            try
            {
                return db.AspNetRoles.OrderByDescending(x => x.Name);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
