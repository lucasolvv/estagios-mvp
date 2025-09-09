using Microsoft.EntityFrameworkCore;
using PlataformaEstagios.Domain.Entities;
using PlataformaEstagios.Domain.Enums;
namespace PlataformaEstagios.Infrastructure.DataAccess {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users => Set<User>();
        public DbSet<Candidate> Candidates => Set<Candidate>();
        public DbSet<Enterprise> Enterprises => Set<Enterprise>();
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<Vacancy> Vacancies => Set<Vacancy>();
        public DbSet<Application> Applications => Set<Application>();
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // --- Candidate ---
            modelBuilder.Entity<Candidate>(e => {
                e.ToTable("Candidates");
                e.HasKey(c => c.Id);
                e.Property(c => c.Id).ValueGeneratedOnAdd();
                e.Property(c => c.CandidateIdentifier).IsRequired().ValueGeneratedNever();
                // ✅ CandidateIdentifier será a Alternate Key (alvo do FK)
                e.HasAlternateKey(c => c.CandidateIdentifier);
                e.Property(c => c.Name).IsRequired().HasMaxLength(120);
                e.Property(c => c.CourseName).HasMaxLength(120);
                // 1:1 Candidate ↔ User usando UserIdentifier (alternate key do User)
                e.HasIndex(c => c.UserIdentifier).IsUnique();
                e.HasOne<User>()
                .WithOne()
                .HasPrincipalKey<User>(u => u.UserIdentifier)
                .HasForeignKey<Candidate>(c => c.UserIdentifier)
                .OnDelete(DeleteBehavior.Restrict);
                e.Property(c => c.Active).HasDefaultValue(true);
                e.Property(c => c.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");
            });

            // --- Enterprise ---
            modelBuilder.Entity<Enterprise>(e => {
                e.ToTable("Enterprises"); e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.EnterpriseIdentifier).IsRequired().ValueGeneratedNever();
                // ✅ EnterpriseIdentifier como Alternate Key
                e.HasAlternateKey(x => x.EnterpriseIdentifier);
                e.Property(x => x.EnterpriseName).HasMaxLength(150);
                e.Property(x => x.Cnpj).HasMaxLength(18);
                e.Property(x => x.ActivityArea).HasMaxLength(120);
                e.HasIndex(x => x.UserIdentifier).IsUnique();
                e.HasOne<User>()
                .WithOne()
                .HasPrincipalKey<User>(u => u.UserIdentifier)
                .HasForeignKey<Enterprise>(x => x.UserIdentifier)
                .OnDelete(DeleteBehavior.Restrict);
                e.Property(x => x.Active).HasDefaultValue(true);
                e.Property(x => x.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");
            });

            // --- Address ---

            modelBuilder.Entity<Address>(e => {
                e.ToTable("Addresses");
                e.HasKey(a => a.Id);
                e.Property(a => a.Id).ValueGeneratedOnAdd();
                e.Property(a => a.AddressIdentifier).IsRequired().ValueGeneratedNever();
                e.Property(a => a.Street).HasMaxLength(120);
                e.Property(a => a.Complement).HasMaxLength(80);
                e.Property(a => a.Neighborhood).HasMaxLength(80);
                e.Property(a => a.City).HasMaxLength(80);
                e.Property(a => a.UF).HasMaxLength(2);
                e.Property(a => a.CEP).HasMaxLength(9);
                // Shadow FKs (opcionais)
                e.Property<Guid?>("CandidateIdentifier");
                e.Property<Guid?>("EnterpriseIdentifier");

                // ✅ 1:1 Address -> Candidate usando a ALTERNATE KEY CandidateIdentifier
                e.HasOne<Candidate>()
                .WithOne(c => c.Address)
                .HasForeignKey<Address>("CandidateIdentifier")
                .HasPrincipalKey<Candidate>(c => c.CandidateIdentifier)
                .OnDelete(DeleteBehavior.Cascade);

                // ✅ 1:1 Address -> Enterprise usando a ALTERNATE KEY EnterpriseIdentifier
                e.HasOne<Enterprise>().WithOne(x => x.Address)
                .HasForeignKey<Address>("EnterpriseIdentifier")
                .HasPrincipalKey<Enterprise>(x => x.EnterpriseIdentifier)
                .OnDelete(DeleteBehavior.Cascade);

                // Índices únicos para manter 1:1
                e.HasIndex("CandidateIdentifier").IsUnique();
                e.HasIndex("EnterpriseIdentifier").IsUnique();

                // XOR: endereço pertence a um OU outro
                e.HasCheckConstraint("CK_Addresses_Owner", "(\"CandidateIdentifier\" IS NOT NULL AND \"EnterpriseIdentifier\" IS NULL) " + "OR (\"CandidateIdentifier\" IS NULL AND \"EnterpriseIdentifier\" IS NOT NULL)");
                e.Property(a => a.Active).HasDefaultValue(true);
                e.Property(a => a.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"); });
            // Se ainda não vai mapear essas entidades agora:

            modelBuilder.Entity<Vacancy>(e =>
            {
                e.ToTable("Vacancies");
                e.HasKey(x => x.VacancyIdentifier);
                e.Property(x => x.Title).HasMaxLength(120).IsRequired();
                e.Property(x => x.Description).HasMaxLength(4000);
                e.Property(x => x.IsActive).HasDefaultValue(true);
                e.Property(x => x.UpdatedAt);

                //se houver entidade Enterprise mapeada
                 e.HasOne<Enterprise>()
                  .WithMany(e => e.Vacancies)
                  .HasForeignKey(x => x.EnterpriseIdentifier)
                  .HasPrincipalKey(nameof(Enterprise.EnterpriseIdentifier));
            });
            
            modelBuilder.Entity<Application>(e =>
            {
                e.ToTable("Applications");

                e.HasKey(x => x.ApplicationIdentifier);
                e.Property(x => x.ApplicationIdentifier).ValueGeneratedNever();

                e.Property(x => x.VacancyId).IsRequired();
                e.Property(x => x.CandidateIdentifier).IsRequired();
                e.Property(x => x.ApplicationDate).IsRequired();
                e.Property(x => x.Status).IsRequired();

                // FK Application.VacancyId -> Vacancy.VacancyIdentifier (PK Guid) - ok
                e.HasOne(x => x.Vacancy)
                 .WithMany(v => v.Applications!)
                 .HasForeignKey(x => x.VacancyId)
                 .OnDelete(DeleteBehavior.Cascade);

                // FK Application.CandidateIdentifier (Guid) -> Candidate.CandidateIdentifier (Alternate Key Guid)
                e.HasOne(x => x.Candidate)
                 .WithMany() // ou .WithMany(c => c.Applications) se existir a coleção
                 .HasForeignKey(x => x.CandidateIdentifier)
                 .HasPrincipalKey(nameof(Candidate.CandidateIdentifier)) // <- ESSA LINHA É O PULO DO GATO
                 .OnDelete(DeleteBehavior.Restrict);

                // Evitar duplicidade: um candidato por vaga
                e.HasIndex(x => new { x.VacancyId, x.CandidateIdentifier }).IsUnique();
            });
            
            
            //modelBuilder.Entity<Candidate>().Ignore(c => c.Applications);
            //modelBuilder.Entity<Enterprise>().Ignore(e => e.Vacancies);
            //modelBuilder.Ignore<Vacancy>();
            } 
    } 
}