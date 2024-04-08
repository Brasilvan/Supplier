using Microsoft.EntityFrameworkCore;
using Supplier.Business.Models;

namespace Supplier.Data.Context
{
    public class MeuDbContext : DbContext
    {        
        public MeuDbContext(DbContextOptions<MeuDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MeuDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
