using JunjiIto.Models;
using Microsoft.EntityFrameworkCore;

namespace JunjiIto.Data
{
    public class LibroContext : DbContext
    {
        public LibroContext(DbContextOptions<LibroContext> o)
        : base(o) { }
        public DbSet<Libro> LibroSet { get; set; }
    }
}
