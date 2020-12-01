using Microsoft.EntityFrameworkCore;

namespace MediatR.Sandbox.CustomerServiceApi.Data
{
    public class MediatRDbContext : DbContext
    {
        public MediatRDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
        }
    }
}