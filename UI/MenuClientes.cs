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
        if (string.IsNullOrWhiteSpace(nombres))
        {
            Console.WriteLine("❌ El nombre no puede estar vacío.");
            Console.ReadKey();
            return;
        }

        Console.Write("Apellidos: ");
        var apellidos = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(apellidos))
        {
            Console.WriteLine("❌ El apellido no puede estar vacío.");
            Console.ReadKey();
            return;
        }

        // Tipo de documento con opciones claras
        Console.WriteLine("Tipo de documento:");
        Console.WriteLine("  [1] Cédula de Ciudadanía (CC)");
        Console.WriteLine("  [2] Pasaporte");
        Console.WriteLine("  [3] Cédula de Extranjería (CE)");
        Console.Write("Opción: ");
        var tipoDoc = Console.ReadLine() switch
        {
            "1" => "CC",
            "2" => "PASAPORTE",
            "3" => "CE",
            _   => ""
        };

        if (string.IsNullOrEmpty(tipoDoc))
        {
            Console.WriteLine("❌ Tipo de documento inválido.");
            Console.ReadKey();
            return;
        }

        // Validar número de documento según tipo
        Console.Write("Número de documento: ");
        var numero = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(numero))
        {
            Console.WriteLine("❌ El número de documento no puede estar vacío.");
            Console.ReadKey();
            return;
        }

        bool documentoValido = tipoDoc switch
        {
            // CC: solo números, entre 6 y 10 dígitos
            "CC" => numero.All(char.IsDigit) && numero.Length >= 6 && numero.Length <= 10,
            // Pasaporte: letras y números, entre 5 y 9 caracteres
            "PASAPORTE" => numero.All(char.IsLetterOrDigit) && numero.Length >= 5 && numero.Length <= 9,
            // CE: letras y números, entre 6 y 10 caracteres
            "CE" => numero.All(char.IsLetterOrDigit) && numero.Length >= 6 && numero.Length <= 10,
            _ => false
        };

        if (!documentoValido)
        {
            Console.WriteLine(tipoDoc switch
            {
                "CC"        => "❌ CC inválida. Solo números, entre 6 y 10 dígitos.",
                "PASAPORTE" => "❌ Pasaporte inválido. Letras y números, entre 5 y 9 caracteres.",
                "CE"        => "❌ CE inválida. Letras y números, entre 6 y 10 caracteres.",
                _           => "❌ Documento inválido."
            });
            Console.ReadKey();
            return;
        }

        // Validar email
        Console.Write("Email: ");
        var email = Console.ReadLine() ?? "";

        if (!email.Contains('@') || !email.Contains('.') || 
            email.IndexOf('@') == 0 || 
            email.IndexOf('@') == email.Length - 1 ||
            email.Split('@')[1].Length < 3)
        {
            Console.WriteLine("❌ Email inválido. Debe tener formato ejemplo@correo.com");
            Console.ReadKey();
            return;
        }

        // Validar teléfono
        Console.Write("Teléfono (10 dígitos): ");
        var telefono = Console.ReadLine() ?? "";

        if (!telefono.All(char.IsDigit) || telefono.Length != 10)
        {
            Console.WriteLine("❌ Teléfono inválido. Debe tener exactamente 10 dígitos.");
            Console.ReadKey();
            return;
        }

        try
        {
            service.Registrar(new Cliente
            {
                Nombres         = nombres.Trim(),
                Apellidos       = apellidos.Trim(),
                TipoDocumento   = tipoDoc,
                NumeroDocumento = numero.Trim(),
                Email           = email.Trim().ToLower(),
                Telefono        = telefono.Trim()
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