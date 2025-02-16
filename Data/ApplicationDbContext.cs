using EstateFInder.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EstateFInder.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        public DbSet<Agent> Agents { get; set; }
    }
}
