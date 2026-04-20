using Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Services;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.UI;

public static class MenuClientes
{
    public static void Mostrar()
    {
        var context = DbContextFactory.Create();
        var service = new ClienteService(context);

        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("========== CLIENTES ==========");
            Console.WriteLine("1. Registrar cliente");
            Console.WriteLine("2. Listar clientes");
            Console.WriteLine("3. Buscar por documento");
            Console.WriteLine("4. Actualizar cliente");
            Console.WriteLine("0. Volver");
            Console.Write("\nOpción: ");

            switch (Console.ReadLine())
            {
                case "1": Registrar(service); break;
                case "2": Listar(service); break;
                case "3": BuscarPorDocumento(service); break;
                case "4": Actualizar(service); break;
                case "0": salir = true; break;
                default:
                    Console.WriteLine("Opción inválida.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void Registrar(ClienteService service)
    {
        Console.Clear();
        Console.WriteLine("--- Registrar Cliente ---\n");

        Console.Write("Nombres: ");
        var nombres = Console.ReadLine() ?? "";
        Console.Write("Apellidos: ");
        var apellidos = Console.ReadLine() ?? "";
        Console.Write("Tipo documento (CC / PASAPORTE / CE): ");
        var tipo = Console.ReadLine() ?? "";
        Console.Write("Número de documento: ");
        var numero = Console.ReadLine() ?? "";
        Console.Write("Email: ");
        var email = Console.ReadLine() ?? "";
        Console.Write("Teléfono: ");
        var telefono = Console.ReadLine() ?? "";

        try
        {
            service.Registrar(new Cliente
            {
                Nombres = nombres,
                Apellidos = apellidos,
                TipoDocumento = tipo.ToUpper(),
                NumeroDocumento = numero,
                Email = email,
                Telefono = telefono
            });
            Console.WriteLine("\n✅ Cliente registrado correctamente.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
        }

        Console.ReadKey();
    }

    private static void Listar(ClienteService service)
    {
        Console.Clear();
        Console.WriteLine("--- Clientes registrados ---\n");
        var lista = service.ObtenerTodos();

        if (!lista.Any())
            Console.WriteLine("No hay clientes registrados.");
        else
            foreach (var c in lista)
                Console.WriteLine($"[{c.Id}] {c.Nombres} {c.Apellidos} | " +
                    $"{c.TipoDocumento}: {c.NumeroDocumento} | {c.Email}");

        Console.ReadKey();
    }

    private static void BuscarPorDocumento(ClienteService service)
    {
        Console.Clear();
        Console.Write("Número de documento: ");
        var doc = Console.ReadLine() ?? "";
        var cliente = service.ObtenerPorDocumento(doc);

        if (cliente == null)
            Console.WriteLine("❌ Cliente no encontrado.");
        else
            Console.WriteLine($"\n[{cliente.Id}] {cliente.Nombres} {cliente.Apellidos}\n" +
                $"Documento: {cliente.TipoDocumento} {cliente.NumeroDocumento}\n" +
                $"Email: {cliente.Email} | Tel: {cliente.Telefono}");

        Console.ReadKey();
    }

    private static void Actualizar(ClienteService service)
    {
        Listar(service);
        Console.Write("ID del cliente a actualizar: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        var cliente = service.ObtenerPorId(id);
        if (cliente == null)
        {
            Console.WriteLine("❌ Cliente no encontrado.");
            Console.ReadKey();
            return;
        }

        Console.Write($"Nuevo email ({cliente.Email}): ");
        var email = Console.ReadLine();
        Console.Write($"Nuevo teléfono ({cliente.Telefono}): ");
        var telefono = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(email)) cliente.Email = email;
        if (!string.IsNullOrWhiteSpace(telefono)) cliente.Telefono = telefono;

        service.Actualizar(cliente);
        Console.WriteLine("\n✅ Cliente actualizado.");
        Console.ReadKey();
    }
}