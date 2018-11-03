//-----------------------------------------------------------------------
// <copyright file="ValidationsRol.cs" company="COEF">
//    Todos los derechos reservados
// </copyright>
//-----------------------------------------------------------------------

namespace CapaNegocio.Validations
{
    using CapaDominio;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="ValidationsRol" />
    /// </summary>
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
