using Microsoft.EntityFrameworkCore;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;

public class AppDbContext : DbContext
{
    public DbSet<Aerolinea> Aerolineas { get; set; }
    public DbSet<Aeropuerto> Aeropuertos { get; set; }
    public DbSet<Vuelo> Vuelos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<Tiquete> Tiquetes { get; set; }
    public DbSet<Pago> Pagos { get; set; }
    public DbSet<ClaseVuelo> ClasesVuelo { get; set; }
    public DbSet<Asiento> Asientos { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Vuelo tiene dos relaciones con Aeropuerto, hay que decirle a EF cuál es cuál
        modelBuilder.Entity<Vuelo>()
            .HasOne(v => v.AeropuertoOrigen)
            .WithMany(a => a.VuelosOrigen)
            .HasForeignKey(v => v.AeropuertoOrigenId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Vuelo>()
            .HasOne(v => v.AeropuertoDestino)
            .WithMany(a => a.VuelosDestino)
            .HasForeignKey(v => v.AeropuertoDestinoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Reserva tiene solo un Tiquete y un Pago
        modelBuilder.Entity<Reserva>()
            .HasOne(r => r.Tiquete)
            .WithOne(t => t.Reserva)
            .HasForeignKey<Tiquete>(t => t.ReservaId);

        modelBuilder.Entity<Reserva>()
            .HasOne(r => r.Pago)
            .WithOne(p => p.Reserva)
            .HasForeignKey<Pago>(p => p.ReservaId);

        // Decimales con precisión explícita para MySQL
        modelBuilder.Entity<Vuelo>()
            .Property(v => v.PrecioPorAsiento)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Reserva>()
            .Property(r => r.ValorTotal)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Pago>()
            .Property(p => p.Monto)
            .HasPrecision(10, 2);
        // Asiento → Vuelo
        modelBuilder.Entity<Asiento>()
            .HasOne(a => a.Vuelo)
            .WithMany(v => v.Asientos)
            .HasForeignKey(a => a.VueloId)
            .OnDelete(DeleteBehavior.Cascade);

        // Asiento → ClaseVuelo
        modelBuilder.Entity<Asiento>()
            .HasOne(a => a.ClaseVuelo)
            .WithMany(c => c.Asientos)
            .HasForeignKey(a => a.ClaseVueloId)
            .OnDelete(DeleteBehavior.Restrict);

        // Cambiar PrecioPorAsiento por PrecioBase si no existe aún
        modelBuilder.Entity<Vuelo>()
            .Property(v => v.PrecioPorAsiento)
            .HasPrecision(10, 2);

        // Seed de las 3 clases de vuelo
        modelBuilder.Entity<ClaseVuelo>().HasData(
            new ClaseVuelo { Id = 1, Nombre = "Economica",     Multiplicador = 1.0m },
            new ClaseVuelo { Id = 2, Nombre = "Ejecutiva",     Multiplicador = 1.8m },
            new ClaseVuelo { Id = 3, Nombre = "Primera Clase", Multiplicador = 3.0m }
        );
    }
}