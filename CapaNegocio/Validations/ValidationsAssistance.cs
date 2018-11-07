using CapaDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Validations
{
    public class ValidationsAssistance : Datamodel
    {
        public IEnumerable<Assistances> GetAllAsistences()
        {

            try
            {
                return db.Assistances;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Assistances SearchById(int? id)
        {
            try
            {
                return db.Assistances.Find(id);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
