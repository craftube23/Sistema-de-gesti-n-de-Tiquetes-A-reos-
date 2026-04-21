using Microsoft.EntityFrameworkCore;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Services;

public class ReporteService
{
    private readonly AppDbContext _context;

    public ReporteService(AppDbContext context)
    {
        _context = context;
    }

    // 1. Vuelos con asientos disponibles ordenados por fecha
    public void VuelosDisponibles()
    {
        var resultado = _context.Vuelos
            .Include(v => v.Aerolinea)
            .Include(v => v.AeropuertoOrigen)
            .Include(v => v.AeropuertoDestino)
            .Where(v => v.AsientosDisponibles > 0 && v.Estado == "PROGRAMADO")
            .OrderBy(v => v.FechaSalida)
            .Select(v => new
            {
                v.CodigoVuelo,
                Origen = v.AeropuertoOrigen.CodigoIATA,
                Destino = v.AeropuertoDestino.CodigoIATA,
                v.FechaSalida,
                v.AsientosDisponibles,
                v.CapacidadTotal,
                Aerolinea = v.Aerolinea.Nombre
            })
            .ToList();

        Console.WriteLine($"\n{"Código",-10} {"Ruta",-12} {"Salida",-18} {"Disponibles",-12} {"Aerolínea",-20}");
        Console.WriteLine(new string('-', 74));

        foreach (var v in resultado)
            Console.WriteLine($"{v.CodigoVuelo,-10} " +
                $"{v.Origen} → {v.Destino,-6} " +
                $"{v.FechaSalida:yyyy-MM-dd HH:mm,-18} " +
                $"{v.AsientosDisponibles}/{v.CapacidadTotal,-8} " +
                $"{v.Aerolinea,-20}");

        Console.WriteLine($"\nTotal: {resultado.Count} vuelo(s) disponible(s).");
    }

    // 2. Destinos más solicitados
    public void DestinosMasSolicitados()
    {
        var resultado = _context.Reservas
            .Include(r => r.Vuelo)
                .ThenInclude(v => v.AeropuertoDestino)
            .GroupBy(r => new
            {
                r.Vuelo.AeropuertoDestino.Ciudad,
                r.Vuelo.AeropuertoDestino.CodigoIATA
            })
            .Select(g => new
            {
                Destino = g.Key.Ciudad,
                Codigo = g.Key.CodigoIATA,
                TotalReservas = g.Count(),
                TotalAsientos = g.Sum(r => r.CantidadAsientos)
            })
            .OrderByDescending(x => x.TotalReservas)
            .ToList();

        Console.WriteLine($"\n{"Destino",-25} {"IATA",-8} {"Reservas",-10} {"Asientos"}");
        Console.WriteLine(new string('-', 55));

        foreach (var d in resultado)
            Console.WriteLine($"{d.Destino,-25} {d.Codigo,-8} {d.TotalReservas,-10} {d.TotalAsientos}");

        Console.WriteLine($"\nTotal: {resultado.Count} destino(s).");
    }

    // 3. Clientes con más reservas
    public void ClientesFrecuentes()
    {
        var resultado = _context.Reservas
            .Include(r => r.Cliente)
            .GroupBy(r => new
            {
                r.Cliente.NumeroDocumento,
                r.Cliente.Nombres,
                r.Cliente.Apellidos
            })
            .Select(g => new
            {
                Cliente = g.Key.Nombres + " " + g.Key.Apellidos,
                Documento = g.Key.NumeroDocumento,
                TotalReservas = g.Count(),
                TotalGastado = g.Sum(r => r.ValorTotal)
            })
            .OrderByDescending(x => x.TotalReservas)
            .ToList();

        Console.WriteLine($"\n{"Cliente",-30} {"Documento",-15} {"Reservas",-10} {"Total gastado"}");
        Console.WriteLine(new string('-', 70));

        foreach (var c in resultado)
            Console.WriteLine($"{c.Cliente,-30} {c.Documento,-15} {c.TotalReservas,-10} ${c.TotalGastado:N0}");

        Console.WriteLine($"\nTotal: {resultado.Count} cliente(s).");
    }

    // 4. Ingresos estimados por aerolínea
    public void IngresosPorAerolinea()
    {
        var resultado = _context.Reservas
            .Include(r => r.Vuelo)
                .ThenInclude(v => v.Aerolinea)
            .Where(r => r.EstadoReserva == "CONFIRMADA")
            .GroupBy(r => r.Vuelo.Aerolinea.Nombre)
            .Select(g => new
            {
                Aerolinea = g.Key,
                TotalReservas = g.Count(),
                Ingresos = g.Sum(r => r.ValorTotal)
            })
            .OrderByDescending(x => x.Ingresos)
            .ToList();

        Console.WriteLine($"\n{"Aerolínea",-25} {"Reservas",-10} {"Ingresos estimados"}");
        Console.WriteLine(new string('-', 55));

        foreach (var a in resultado)
            Console.WriteLine($"{a.Aerolinea,-25} {a.TotalReservas,-10} ${a.Ingresos:N0}");

        var totalGeneral = resultado.Sum(r => r.Ingresos);
        Console.WriteLine(new string('-', 55));
        Console.WriteLine($"{"TOTAL",-35} ${totalGeneral:N0}");
    }

    // 5. Reservas por estado
    public void ReservasPorEstado()
    {
        var resultado = _context.Reservas
            .GroupBy(r => r.EstadoReserva)
            .Select(g => new
            {
                Estado = g.Key,
                Cantidad = g.Count(),
                ValorTotal = g.Sum(r => r.ValorTotal)
            })
            .OrderByDescending(x => x.Cantidad)
            .ToList();

        Console.WriteLine($"\n{"Estado",-15} {"Cantidad",-10} {"Valor total"}");
        Console.WriteLine(new string('-', 40));

        foreach (var r in resultado)
            Console.WriteLine($"{r.Estado,-15} {r.Cantidad,-10} ${r.ValorTotal:N0}");
    }

    // 6. Vuelos con mayor ocupación
    public void VuelosMayorOcupacion()
    {
        var resultado = _context.Vuelos
            .Include(v => v.Aerolinea)
            .Include(v => v.AeropuertoOrigen)
            .Include(v => v.AeropuertoDestino)
            .Where(v => v.CapacidadTotal > 0)
            .Select(v => new
            {
                v.CodigoVuelo,
                Origen = v.AeropuertoOrigen.CodigoIATA,
                Destino = v.AeropuertoDestino.CodigoIATA,
                v.CapacidadTotal,
                Ocupados = v.CapacidadTotal - v.AsientosDisponibles,
                Porcentaje = (double)(v.CapacidadTotal - v.AsientosDisponibles)
                             / v.CapacidadTotal * 100,
                Aerolinea = v.Aerolinea.Nombre
            })
            .OrderByDescending(x => x.Porcentaje)
            .ToList();

        Console.WriteLine($"\n{"Código",-10} {"Ruta",-12} {"Ocupados",-10} {"Capacidad",-10} {"% Ocupación",-12} {"Aerolínea"}");
        Console.WriteLine(new string('-', 70));

        foreach (var v in resultado)
            Console.WriteLine($"{v.CodigoVuelo,-10} " +
                $"{v.Origen} → {v.Destino,-6} " +
                $"{v.Ocupados,-10} " +
                $"{v.CapacidadTotal,-10} " +
                $"{v.Porcentaje:F1}%{"",-6} " +
                $"{v.Aerolinea}");
    }

    // 7. Tiquetes emitidos por rango de fechas
    public void TiquetesPorRangoFechas(DateTime desde, DateTime hasta)
    {
        var resultado = _context.Tiquetes
            .Include(t => t.Reserva)
                .ThenInclude(r => r.Cliente)
            .Include(t => t.Reserva)
                .ThenInclude(r => r.Vuelo)
                    .ThenInclude(v => v.AeropuertoOrigen)
            .Include(t => t.Reserva)
                .ThenInclude(r => r.Vuelo)
                    .ThenInclude(v => v.AeropuertoDestino)
            .Where(t => t.FechaEmision >= desde && t.FechaEmision <= hasta)
            .OrderBy(t => t.FechaEmision)
            .ToList();

        Console.WriteLine($"\nTiquetes emitidos entre {desde:yyyy-MM-dd} y {hasta:yyyy-MM-dd}:\n");
        Console.WriteLine($"\n{"Código",-20} {"Pasajero",-25} {"Ruta",-12} {"Emitido",-18} {"Estado"}");
        Console.WriteLine(new string('-', 85));

        foreach (var t in resultado)
            Console.WriteLine($"{t.CodigoTiquete,-20} " +
                $"{t.Reserva.Cliente.Nombres + " " + t.Reserva.Cliente.Apellidos,-25} " +
                $"{t.Reserva.Vuelo.AeropuertoOrigen.CodigoIATA} → {t.Reserva.Vuelo.AeropuertoDestino.CodigoIATA,-6} " +
                $"{t.FechaEmision:yyyy-MM-dd HH:mm,-18} " +
                $"{t.Estado}");

        Console.WriteLine($"\nTotal: {resultado.Count} tiquete(s) en ese rango.");
    }
}