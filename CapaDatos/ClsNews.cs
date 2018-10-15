using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class ClsNews
    {
        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }
        [Display(Name = "Fecha noticia")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime Date { get; set; }
        [Required]
        [Display(Name = "Activo")]
        public bool Active { get; set; }
        [Required]
        [Display(Name = "Codigo")]
        public string Id { get; set; }

    }
    [MetadataType(typeof(ClsNews))]
    public partial class News
    {

    }
}
