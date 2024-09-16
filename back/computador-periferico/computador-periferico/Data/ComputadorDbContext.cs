using computador_periferico.Data.Entidades;
using Microsoft.EntityFrameworkCore;

namespace computador_periferico.Data;

public class ComputadorDbContext : DbContext
{

    public DbSet<Computador> Computadores => Set<Computador>();
    public DbSet<Periferico> Perifericos => Set<Periferico>();

    public ComputadorDbContext()
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=computador.sqlite");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Computador>()
            .HasKey(x => x.Id);
        modelBuilder.Entity<Periferico>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Computador>()
            .HasMany(x => x.Perifericos)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Periferico>()
            .HasOne(x => x.Computador)
            .WithMany()
            .HasForeignKey(v => v.ComputadorId);

        modelBuilder.Entity<Periferico>().Ignore(x => x.Computador);

        base.OnModelCreating(modelBuilder);
    }
}
