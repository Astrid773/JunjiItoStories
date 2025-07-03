using JunjiIto.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JunjiIto.ViewModels
{
    public class BuscaVM
    {
        public List<Libro> LosLibros { get; set; }
        
        public string BNom {  get; set; }
        public string BPublicacion { get; set; }
    }
}
