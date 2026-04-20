using Microsoft.EntityFrameworkCore;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Services;

public class ReservaService
{
    private readonly AppDbContext _context;

    public ReservaService(AppDbContext context)
    {
        _context = context;
    }

    public void Crear(Reserva reserva)
    {
        // Validar disponibilidad de asientos
        var vuelo = _context.Vuelos.FirstOrDefault(v => v.Id == reserva.VueloId);

        if (vuelo == null)
            throw new InvalidOperationException("El vuelo no existe.");

        if (vuelo.Estado != "PROGRAMADO")
            throw new InvalidOperationException(
                $"El vuelo no está disponible. Estado actual: {vuelo.Estado}");

        if (vuelo.AsientosDisponibles < reserva.CantidadAsientos)
            throw new InvalidOperationException(
                $"No hay suficientes asientos. Disponibles: {vuelo.AsientosDisponibles}");

        // Calcular valor total
        reserva.ValorTotal = vuelo.PrecioPorAsiento * reserva.CantidadAsientos;
        reserva.CodigoReserva = GenerarCodigo();
        reserva.EstadoReserva = "PENDIENTE";
        reserva.FechaReserva = DateTime.Now;

        // Descontar asientos
        vuelo.AsientosDisponibles -= reserva.CantidadAsientos;

        _context.Reservas.Add(reserva);
        _context.SaveChanges();
    }

    public void Confirmar(int id)
    {
        var reserva = ObtenerPorId(id);

        if (reserva == null)
            throw new InvalidOperationException("Reserva no encontrada.");

        if (reserva.EstadoReserva != "PENDIENTE")
            throw new InvalidOperationException(
                $"Solo se pueden confirmar reservas PENDIENTES. Estado actual: {reserva.EstadoReserva}");

        reserva.EstadoReserva = "CONFIRMADA";
        _context.SaveChanges();
    }

    public void Cancelar(int id)
    {
        var reserva = ObtenerPorId(id);

        if (reserva == null)
            throw new InvalidOperationException("Reserva no encontrada.");

        if (reserva.EstadoReserva == "CANCELADA")
            throw new InvalidOperationException("La reserva ya está cancelada.");

        // Devolver asientos al vuelo
        var vuelo = _context.Vuelos.FirstOrDefault(v => v.Id == reserva.VueloId);
        if (vuelo != null)
            vuelo.AsientosDisponibles += reserva.CantidadAsientos;

        reserva.EstadoReserva = "CANCELADA";
        _context.SaveChanges();
    }

    public List<Reserva> ObtenerTodas()
    {
        return _context.Reservas
            .Include(r => r.Cliente)
            .Include(r => r.Vuelo)
                .ThenInclude(v => v.AeropuertoOrigen)
            .Include(r => r.Vuelo)
                .ThenInclude(v => v.AeropuertoDestino)
            .Include(r => r.Vuelo)
                .ThenInclude(v => v.Aerolinea)
            .ToList();
    }

    public Reserva? ObtenerPorId(int id)
    {
        return _context.Reservas
            .Include(r => r.Cliente)
            .Include(r => r.Vuelo)
                .ThenInclude(v => v.AeropuertoOrigen)
            .Include(r => r.Vuelo)
                .ThenInclude(v => v.AeropuertoDestino)
            .Include(r => r.Vuelo)
                .ThenInclude(v => v.Aerolinea)
            .FirstOrDefault(r => r.Id == id);
    }

    public List<Reserva> ObtenerPorCliente(int clienteId)
    {
        return _context.Reservas
            .Include(r => r.Vuelo)
                .ThenInclude(v => v.AeropuertoOrigen)
            .Include(r => r.Vuelo)
                .ThenInclude(v => v.AeropuertoDestino)
            .Where(r => r.ClienteId == clienteId)
            .ToList();
    }

    private static string GenerarCodigo()
    {
        return "RES-" + DateTime.Now.ToString("yyyyMMddHHmmss") +
               "-" + new Random().Next(100, 999);
    }
}