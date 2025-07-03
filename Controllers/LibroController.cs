using JunjiIto.Data;
using JunjiIto.Models;
using JunjiIto.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace JunjiIto.Controllers
{
    public class LibroController : Controller
    {
        private readonly LibroContext _context;
        private readonly IWebHostEnvironment _environment;

        public LibroController(LibroContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public async Task<IActionResult> Index(string ordenaP, string bNom, string bPublicacion)
        {
            var lib = from lista in _context.LibroSet
                      select lista;

            ViewData["PubliSort"] = String.IsNullOrEmpty(ordenaP) ||
                ordenaP == "des" ? "asc" : "des";

            switch (ordenaP)
            {
                case "des":
                    lib = lib.OrderByDescending(p => p.Publicacion);
                    break;
                case "asc":
                    lib = lib.OrderBy(p => p.Publicacion);
                    break;
                default:
                    break;
            }

            if(!String.IsNullOrEmpty(bNom))
            {
                lib = lib.Where(n => n.Nombre.Contains(bNom));
            }
            if(!String.IsNullOrEmpty(bPublicacion))
            {
                lib = lib.Where(p => p.Publicacion.Contains(bPublicacion));
            }

            var vaca = new BuscaVM()
            {
                LosLibros = await lib.ToListAsync()
            };
            return View(vaca);
        }

        public IActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Nuevo(LibroVM model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = LoadedFile(model);
                Libro libroNuevo = new Libro()
                {
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion,
                    Publicacion = model.Publicacion,
                    FotoLibro = uniqueFileName
                };
                _context.Add(libroNuevo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private string LoadedFile(LibroVM model)
        {
            string uniqueFileName = null;
            if (model.Foto != null)
            {
                string uploadFolder = Path.Combine(_environment.WebRootPath, "Portadas");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Foto.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Foto.CopyTo(fileStream);
                };
            }
            else
            {
                uniqueFileName = "Default.jpg";
            }
            return uniqueFileName;
        }

        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var losLibros = await _context.LibroSet.FirstOrDefaultAsync(x => x.Id == id);
            if (losLibros == null)
            {
                return NotFound();
            }
            return View(losLibros);
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var losLibros = await _context.LibroSet.FindAsync(id);
            if (losLibros == null)
            {
                return NotFound();
            }
            return View(losLibros);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("Id,Nombre,Descripcion,Publicacion,FotoLibro")] Libro libro, IFormFile Foto)
        {
            if(id != libro.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    if (Foto != null)
                    {
                        string uploadFolder = Path.Combine(_environment.WebRootPath, "Portadas");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Foto.FileName;
                        string filePath = Path.Combine(uploadFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            Foto.CopyTo(fileStream);
                        }
                        libro.FotoLibro = uniqueFileName;
                    }
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if(!LibroExiste(libro.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        private bool LibroExiste(int id)
        {
            return _context.LibroSet.Any(l => l.Id == id);
        }

        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var libro = await _context.LibroSet.FindAsync(id);
            if (libro == null) return NotFound();

            return View(libro);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var libro = await _context.LibroSet.FindAsync(id);
            if (libro != null)
            {
                _context.LibroSet.Remove(libro);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
