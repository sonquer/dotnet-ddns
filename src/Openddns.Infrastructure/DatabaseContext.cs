using Microsoft.EntityFrameworkCore;
using Openddns.Core.Interfaces;
using Openddns.Core.Models;

namespace Openddns.Infrastructure
{
    public class DatabaseContext : DbContext, IUnitOfWork
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogModel>().ToTable(nameof(Logs));
        }

        public DbSet<LogModel>? Logs { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
