using System.ComponentModel.DataAnnotations;

namespace JunjiIto.ViewModels
{
    public class LibroVM
    {
        [Required]
        [Display(Name = "Nombre de obra")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción de obra")]
        public string Descripcion { get; set; }

        [Display(Name = "Año de publicación")]
        [DataType(DataType.Date)]
        public string Publicacion { get; set; }

        public IFormFile Foto { get; set; }
    }
}
