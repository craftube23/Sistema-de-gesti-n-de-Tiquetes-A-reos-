using Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Services;

public class AeropuertoService
{
    private readonly AppDbContext _context;

    public AeropuertoService(AppDbContext context)
    {
        _context = context;
    }

    public void Registrar(Aeropuerto aeropuerto)
    {
        _context.Aeropuertos.Add(aeropuerto);
        _context.SaveChanges();
    }

    public List<Aeropuerto> ObtenerTodos()
    {
        return _context.Aeropuertos.ToList();
    }

    public Aeropuerto? ObtenerPorId(int id)
    {
        return _context.Aeropuertos.FirstOrDefault(a => a.Id == id);
    }

    public void Actualizar(Aeropuerto aeropuerto)
    {
        _context.Aeropuertos.Update(aeropuerto);
        _context.SaveChanges();
    }
}