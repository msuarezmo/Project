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
    
    public partial class Students
    {
        public Students()
        {
            this.Lacks = new HashSet<Lacks>();
        }
    
        public int IdStudent { get; set; }
        public string Names { get; set; }
        public string Surnames { get; set; }
        public int DocumentTypeId { get; set; }
        public int CourseId { get; set; }
        public string Document { get; set; }
        public string ParentId { get; set; }
        public bool Assistance { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Courses Courses { get; set; }
        public virtual DocumentType DocumentType { get; set; }
        public virtual ICollection<Lacks> Lacks { get; set; }
    }
}
