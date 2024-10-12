using Automix.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Automix.Data
{
    public class AutomixDbContext : DbContext
    {
        public AutomixDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Percursos> Percursos { get; set; }
        public DbSet<Contactos> Contactos { get; set; }
        public DbSet<Moradas> Moradas { get; set; }
        public DbSet<Documentos> Documentos { get; set; }
        public DbSet<Alunos> Alunos { get; set; }
        public DbSet<PercursosAlunos> PercursosAlunos { get; set; }

    }
}
