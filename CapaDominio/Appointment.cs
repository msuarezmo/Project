//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CapaDominio
{
    using System;
    using System.Collections.Generic;
    
    public partial class Appointment
    {
        public int IdAppointment { get; set; }
        public int Issue { get; set; }
        public string Description { get; set; }
        public Nullable<int> HistoricId { get; set; }
        public Nullable<int> StatusId { get; set; }
    
        public virtual Historic Historic { get; set; }
        public virtual Issue Issue1 { get; set; }
        public virtual Status Status { get; set; }
    }
}
