using CapaDominio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.Validations
{
    public class ValidationsDocumenType : Datamodel
    {
        /// <summary>
        /// Devuelve todos los tipos de documento
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DocumentType> GetAllDocumentTypes()
        {
            try
            {
                return db.DocumentType.OrderByDescending(x => x.Name);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
