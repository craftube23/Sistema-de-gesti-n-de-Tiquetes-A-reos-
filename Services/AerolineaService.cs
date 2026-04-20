using Microsoft.EntityFrameworkCore;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Services;

public class AerolineaService
{
    private readonly AppDbContext _context;

    public AerolineaService(AppDbContext context)
    {
        _context = context;
    }

    public void Registrar(Aerolinea aerolinea)
    {
        _context.Aerolineas.Add(aerolinea);
        _context.SaveChanges();
    }

    public List<Aerolinea> ObtenerTodas()
    {
        return _context.Aerolineas.Where(a => a.Activa).ToList();
    }

    public Aerolinea? ObtenerPorId(int id)
    {
        return _context.Aerolineas.FirstOrDefault(a => a.Id == id);
    }

    public void Actualizar(Aerolinea aerolinea)
    {
        _context.Aerolineas.Update(aerolinea);
        _context.SaveChanges();
    }

    public void Desactivar(int id)
    {
        var aerolinea = ObtenerPorId(id);
        if (aerolinea != null)
        {
            aerolinea.Activa = false;
            _context.SaveChanges();
        }
    }
}