using Microsoft.EntityFrameworkCore;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Services;

public class AsientoService
{
    private readonly AppDbContext _context;

    public AsientoService(AppDbContext context)
    {
        _context = context;
    }

    // Genera todos los asientos del vuelo según distribución por clase
    public void GenerarAsientos(int vueloId)
    {
        var vuelo = _context.Vuelos.FirstOrDefault(v => v.Id == vueloId)
            ?? throw new InvalidOperationException("Vuelo no encontrado.");

        var yaExisten = _context.Asientos.Any(a => a.VueloId == vueloId);
        if (yaExisten)
            throw new InvalidOperationException("Este vuelo ya tiene asientos generados.");

        var clases = _context.ClasesVuelo.ToList();
        var primera   = clases.First(c => c.Nombre == "Primera Clase");
        var ejecutiva = clases.First(c => c.Nombre == "Ejecutiva");
        var economica = clases.First(c => c.Nombre == "Economica");

        var asientos = new List<Asiento>();
        int fila = 1;

        // Primera Clase — 3 asientos por fila (A B C)
        for (int i = 0; i < vuelo.AsientosPrimeraClase; i++)
        {
            if (i > 0 && i % 3 == 0) fila++;
            char letra = (char)('A' + (i % 3));
            asientos.Add(new Asiento
            {
                Numero       = $"{fila}{letra}",
                VueloId      = vueloId,
                ClaseVueloId = primera.Id,
                Estado       = "DISPONIBLE"
            });
        }

        fila++;

        // Ejecutiva — 4 asientos por fila (A B C D)
        for (int i = 0; i < vuelo.AsientosEjecutiva; i++)
        {
            if (i > 0 && i % 4 == 0) fila++;
            char letra = (char)('A' + (i % 4));
            asientos.Add(new Asiento
            {
                Numero       = $"{fila}{letra}",
                VueloId      = vueloId,
                ClaseVueloId = ejecutiva.Id,
                Estado       = "DISPONIBLE"
            });
        }

        fila++;

        // Económica — 6 asientos por fila (A B C D E F)
        for (int i = 0; i < vuelo.AsientosEconomica; i++)
        {
            if (i > 0 && i % 6 == 0) fila++;
            char letra = (char)('A' + (i % 6));
            asientos.Add(new Asiento
            {
                Numero       = $"{fila}{letra}",
                VueloId      = vueloId,
                ClaseVueloId = economica.Id,
                Estado       = "DISPONIBLE"
            });
        }

        _context.Asientos.AddRange(asientos);

        // Actualizar total de asientos disponibles en el vuelo
        vuelo.AsientosDisponibles = asientos.Count;
        vuelo.CapacidadTotal      = asientos.Count;

        _context.SaveChanges();
    }

    public List<Asiento> ObtenerPorVuelo(int vueloId)
    {
        return _context.Asientos
            .Include(a => a.ClaseVuelo)
            .Where(a => a.VueloId == vueloId)
            .OrderBy(a => a.ClaseVueloId)
            .ThenBy(a => a.Numero)
            .ToList();
    }

    public List<Asiento> ObtenerDisponiblesPorClase(int vueloId, int claseId)
    {
        return _context.Asientos
            .Include(a => a.ClaseVuelo)
            .Where(a => a.VueloId      == vueloId
                     && a.ClaseVueloId == claseId
                     && a.Estado       == "DISPONIBLE")
            .OrderBy(a => a.Numero)
            .ToList();
    }

    public Asiento? ObtenerPorId(int id)
    {
        return _context.Asientos
            .Include(a => a.ClaseVuelo)
            .FirstOrDefault(a => a.Id == id);
    }

    public decimal CalcularPrecio(int vueloId, int claseId)
    {
        var vuelo = _context.Vuelos.FirstOrDefault(v => v.Id == vueloId)
            ?? throw new InvalidOperationException("Vuelo no encontrado.");

        var clase = _context.ClasesVuelo.FirstOrDefault(c => c.Id == claseId)
            ?? throw new InvalidOperationException("Clase no encontrada.");

        return vuelo.PrecioPorAsiento * clase.Multiplicador;
    }

    public void ReservarAsientos(List<int> asientoIds)
    {
        var asientos = _context.Asientos
            .Where(a => asientoIds.Contains(a.Id))
            .ToList();

        foreach (var asiento in asientos)
        {
            if (asiento.Estado != "DISPONIBLE")
                throw new InvalidOperationException(
                    $"El asiento {asiento.Numero} no está disponible. Estado: {asiento.Estado}");

            asiento.Estado = "RESERVADO";
        }

        _context.SaveChanges();
    }

    public void LiberarAsientos(List<int> asientoIds)
    {
        var asientos = _context.Asientos
            .Where(a => asientoIds.Contains(a.Id))
            .ToList();

        foreach (var asiento in asientos)
            asiento.Estado = "DISPONIBLE";

        _context.SaveChanges();
    }

    public void OcuparAsientos(List<int> asientoIds)
    {
        var asientos = _context.Asientos
            .Where(a => asientoIds.Contains(a.Id))
            .ToList();

        foreach (var asiento in asientos)
            asiento.Estado = "OCUPADO";

        _context.SaveChanges();
    }

    public List<ClaseVuelo> ObtenerClases()
    {
        return _context.ClasesVuelo.ToList();
    }
}