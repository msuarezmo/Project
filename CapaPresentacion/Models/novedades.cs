//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CapaPresentacion.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class novedades
    {
        public int id { get; set; }
        public string tipo { get; set; }
        public Nullable<System.DateTime> fecha_ingreso_de_novedad { get; set; }
        public Nullable<System.DateTime> fecha_solucion { get; set; }
        public string respuesta { get; set; }
        public Nullable<int> id_usuario { get; set; }
    
        public virtual usuarios usuarios { get; set; }
    }
}
