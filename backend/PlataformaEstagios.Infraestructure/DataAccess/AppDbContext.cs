using Microsoft.EntityFrameworkCore;
using PlataformaEstagios.Domain.Entities;

namespace PlataformaEstagios.Infrastructure.DataAccess
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u => u.UserIdentifier);

                entity.Property(u => u.Nickname)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.Password)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.UserType)
                      .IsRequired()
                      .HasConversion<int>(); // map enum to int

                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.Nickname).IsUnique();
            });
        }
    }
}
