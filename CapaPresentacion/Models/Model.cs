using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Models
{
    public partial class Model : DbContext
    {
        public Model()
              : base("name=colegioEntities")
        {
        }

    }
}