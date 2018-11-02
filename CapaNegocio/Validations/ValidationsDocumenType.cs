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
        public List<DocumentType> GetAllDocumentTypes()
        {
            try
            {
                return db.DocumentType.OrderByDescending(x => x.Name).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
