using Microsoft.EntityFrameworkCore;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Services;

public class TiqueteService
{
    private readonly AppDbContext _context;

    public TiqueteService(AppDbContext context)
    {
        _context = context;
    }

    public void Emitir(int reservaId)
    {
        var reserva = _context.Reservas
            .Include(r => r.Tiquete)
            .FirstOrDefault(r => r.Id == reservaId);

        if (reserva == null)
            throw new InvalidOperationException("Reserva no encontrada.");

        if (reserva.EstadoReserva != "CONFIRMADA")
            throw new InvalidOperationException(
                $"Solo se pueden emitir tiquetes de reservas CONFIRMADAS. " +
                $"Estado actual: {reserva.EstadoReserva}");

        if (reserva.Tiquete != null)
            throw new InvalidOperationException(
                $"Esta reserva ya tiene un tiquete emitido: {reserva.Tiquete.CodigoTiquete}");

        var tiquete = new Tiquete
        {
            CodigoTiquete = GenerarCodigo(),
            ReservaId = reservaId,
            FechaEmision = DateTime.Now,
            Estado = "EMITIDO"
        };

        _context.Tiquetes.Add(tiquete);
        _context.SaveChanges();
    }

    public List<Tiquete> ObtenerTodos()
    {
        return _context.Tiquetes
            .Include(t => t.Reserva)
                .ThenInclude(r => r.Cliente)
            .Include(t => t.Reserva)
                .ThenInclude(r => r.Vuelo)
                    .ThenInclude(v => v.AeropuertoOrigen)
            .Include(t => t.Reserva)
                .ThenInclude(r => r.Vuelo)
                    .ThenInclude(v => v.AeropuertoDestino)
            .ToList();
    }

    public Tiquete? ObtenerPorId(int id)
    {
        return _context.Tiquetes
            .Include(t => t.Reserva)
                .ThenInclude(r => r.Cliente)
            .Include(t => t.Reserva)
                .ThenInclude(r => r.Vuelo)
                    .ThenInclude(v => v.AeropuertoOrigen)
            .Include(t => t.Reserva)
                .ThenInclude(r => r.Vuelo)
                    .ThenInclude(v => v.AeropuertoDestino)
            .FirstOrDefault(t => t.Id == id);
    }

    public void CambiarEstado(int id, string estado)
    {
        var tiquete = _context.Tiquetes.FirstOrDefault(t => t.Id == id);

        if (tiquete == null)
            throw new InvalidOperationException("Tiquete no encontrado.");

        tiquete.Estado = estado;
        _context.SaveChanges();
    }

    private static string GenerarCodigo()
    {
        return "TKT-" + DateTime.Now.ToString("yyyyMMddHHmmss") +
               "-" + new Random().Next(100, 999);
    }
}