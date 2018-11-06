using CapaDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Validations
{
    public class ValidationsLack : Datamodel
    {
        public IEnumerable<Lacks> GetAllLacks()
        {
            try
            {
                return db.Lacks;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
