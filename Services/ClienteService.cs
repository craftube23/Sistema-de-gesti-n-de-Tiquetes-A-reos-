using Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Services;

public class ClienteService
{
    private readonly AppDbContext _context;

    public ClienteService(AppDbContext context)
    {
        _context = context;
    }

    public void Registrar(Cliente cliente)
    {
        // Validar que el documento no esté duplicado
        var existe = _context.Clientes
            .Any(c => c.NumeroDocumento == cliente.NumeroDocumento);

        if (existe)
            throw new InvalidOperationException(
                $"Ya existe un cliente con el documento {cliente.NumeroDocumento}.");

        _context.Clientes.Add(cliente);
        _context.SaveChanges();
    }

    public List<Cliente> ObtenerTodos()
    {
        return _context.Clientes.ToList();
    }

    public Cliente? ObtenerPorId(int id)
    {
        return _context.Clientes.FirstOrDefault(c => c.Id == id);
    }

    public Cliente? ObtenerPorDocumento(string documento)
    {
        return _context.Clientes
            .FirstOrDefault(c => c.NumeroDocumento == documento);
    }

    public void Actualizar(Cliente cliente)
    {
        _context.Clientes.Update(cliente);
        _context.SaveChanges();
    }
}