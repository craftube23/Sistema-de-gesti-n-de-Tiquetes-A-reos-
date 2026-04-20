using Microsoft.EntityFrameworkCore;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Services;

public class VueloService
{
    private readonly AppDbContext _context;

    public VueloService(AppDbContext context)
    {
        _context = context;
    }

    public void Registrar(Vuelo vuelo)
    {
        vuelo.AsientosDisponibles = vuelo.CapacidadTotal;
        _context.Vuelos.Add(vuelo);
        _context.SaveChanges();
    }

    public List<Vuelo> ObtenerTodos()
    {
        return _context.Vuelos
            .Include(v => v.Aerolinea)
            .Include(v => v.AeropuertoOrigen)
            .Include(v => v.AeropuertoDestino)
            .ToList();
    }

    public Vuelo? ObtenerPorId(int id)
    {
        return _context.Vuelos
            .Include(v => v.Aerolinea)
            .Include(v => v.AeropuertoOrigen)
            .Include(v => v.AeropuertoDestino)
            .FirstOrDefault(v => v.Id == id);
    }

    public List<Vuelo> ObtenerDisponibles()
    {
        return _context.Vuelos
            .Include(v => v.Aerolinea)
            .Include(v => v.AeropuertoOrigen)
            .Include(v => v.AeropuertoDestino)
            .Where(v => v.AsientosDisponibles > 0 && v.Estado == "PROGRAMADO")
            .OrderBy(v => v.FechaSalida)
            .ToList();
    }

    public void ActualizarEstado(int id, string estado)
    {
        var vuelo = ObtenerPorId(id);
        if (vuelo != null)
        {
            vuelo.Estado = estado;
            _context.SaveChanges();
        }
    }
}