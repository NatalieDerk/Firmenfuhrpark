using Microsoft.EntityFrameworkCore;

namespace Backend.Db_table
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> opitons) : base(opitons) { }

        public DbSet<Rolle> Rollen { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Standort> Standorte { get; set; }
        public DbSet<Fahrzeuge> Fahrzeuge { get; set; }
        public DbSet<Formular> Formular { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
