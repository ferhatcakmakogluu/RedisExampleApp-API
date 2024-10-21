using Microsoft.EntityFrameworkCore;

namespace RedisExampleApp.Api.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        {
            
        }

        public DbSet<Products> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Products>().HasData(
                new Products { Id=1,Name="Kalem",Price=100},
                new Products { Id=2, Name="Kitap", Price=200},
                new Products { Id=3, Name="Defter", Price=50}
            );


            base.OnModelCreating(modelBuilder);
        }
    }
}
