using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Validations
{
    public class Dispose : Datamodel
    {
        public void Liberate()
        {
            db.Dispose();
        }
    }
}
