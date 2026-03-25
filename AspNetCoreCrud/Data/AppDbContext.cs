using AspNetCoreCrud.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreCrud.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<Estudante> Estudanes { get; set; }
    }
}
