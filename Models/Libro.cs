using System.ComponentModel.DataAnnotations;

namespace JunjiIto.Models
{
    public class Libro
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre de obra")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción de obra")]
        public string Descripcion {  get; set; }

        [Display(Name = "Año de publicación")]
        [DataType(DataType.Date)]
        public string Publicacion { get; set; }

        public string FotoLibro { get; set; }
    }
}
