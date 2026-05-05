using ExemploProva.Models;
using Microsoft.EntityFrameworkCore;

namespace ExemploProva.Data
{
    public class FakeDbContext : DbContext
    {
        public FakeDbContext(DbContextOptions<FakeDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
    }
}
