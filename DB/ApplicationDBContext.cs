using Microsoft.EntityFrameworkCore;

namespace Backend.Db_tables
{
    // Stellt die Verbindung zur PostgreSQL-Datenbank her und definiert alle Tabellen sowie deren Beziehungen
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        // Definition der Datenbanktabellen
        public DbSet<Rolle> Rollen { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Standort> Standorte { get; set; }
        public DbSet<Fahrzeuge> Fahrzeuge { get; set; }
        public DbSet<Formular> Formular { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Definition der Primärschlüssel
            modelBuilder.Entity<Rolle>().HasKey(r => r.IdRolle);
            modelBuilder.Entity<User>().HasKey(u => u.IdUser);
            modelBuilder.Entity<Standort>().HasKey(s => s.IdOrt);
            modelBuilder.Entity<Fahrzeuge>().HasKey(f => f.IdCar);
            modelBuilder.Entity<Formular>().HasKey(f => f.IdForm);

            modelBuilder.Entity<User>()
            .HasOne(u => u.Rolle)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.IdRolle);

            modelBuilder.Entity<Fahrzeuge>()
            .HasOne(f => f.Standort)
            .WithMany(s => s.Fahrzeuge)
            .HasForeignKey(f => f.IdOrt);

            modelBuilder.Entity<Formular>()
            .HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.IdUser)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Formular>()
           .HasOne(f => f.Manager)
           .WithMany()
           .HasForeignKey(f => f.IdManager)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Formular>()
           .HasOne(f => f.Fahrzeuge)
           .WithMany()
           .HasForeignKey(f => f.IdCar)
           .OnDelete(DeleteBehavior.Restrict);
            
             modelBuilder.Entity<Formular>()
            .HasOne(f => f.Standort)
            .WithMany()
            .HasForeignKey(f => f.IdOrt)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
