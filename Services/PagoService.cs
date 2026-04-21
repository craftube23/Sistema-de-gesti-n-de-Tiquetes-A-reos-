using Microsoft.EntityFrameworkCore;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Services;

public class PagoService
{
    private readonly AppDbContext _context;

    public PagoService(AppDbContext context)
    {
        _context = context;
    }

    public void Registrar(int reservaId, string metodoPago)
    {
        var reserva = _context.Reservas
            .Include(r => r.Pago)
            .FirstOrDefault(r => r.Id == reservaId);

        if (reserva == null)
            throw new InvalidOperationException("Reserva no encontrada.");

        if (reserva.EstadoReserva == "CANCELADA")
            throw new InvalidOperationException("No se puede registrar pago de una reserva cancelada.");

        if (reserva.Pago != null)
            throw new InvalidOperationException(
                $"Esta reserva ya tiene un pago registrado. Estado: {reserva.Pago.Estado}");

        var pago = new Pago
        {
            ReservaId = reservaId,
            Monto = reserva.ValorTotal,
            MetodoPago = metodoPago.ToUpper(),
            Estado = "PAGADO",
            FechaPago = DateTime.Now
        };

        _context.Pagos.Add(pago);
        _context.SaveChanges();
    }

    public void CambiarEstado(int pagoId, string estado)
    {
        var pago = _context.Pagos.FirstOrDefault(p => p.Id == pagoId);

        if (pago == null)
            throw new InvalidOperationException("Pago no encontrado.");

        pago.Estado = estado.ToUpper();
        _context.SaveChanges();
    }

    public List<Pago> ObtenerTodos()
    {
        return _context.Pagos
            .Include(p => p.Reserva)
                .ThenInclude(r => r.Cliente)
            .Include(p => p.Reserva)
                .ThenInclude(r => r.Vuelo)
                    .ThenInclude(v => v.Aerolinea)
            .ToList();
    }

    public Pago? ObtenerPorReserva(int reservaId)
    {
        return _context.Pagos
            .Include(p => p.Reserva)
            .FirstOrDefault(p => p.ReservaId == reservaId);
    }
}