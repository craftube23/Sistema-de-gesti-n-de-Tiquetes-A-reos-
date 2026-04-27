using Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Services;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.UI;

public static class MenuAeropuertos
{
    public static void Mostrar()
    {
        var context = DbContextFactory.Create();
        var service = new AeropuertoService(context);

        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("========== AEROPUERTOS ==========");
            Console.WriteLine("1. Registrar aeropuerto");
            Console.WriteLine("2. Listar aeropuertos");
            Console.WriteLine("3. Actualizar aeropuerto");
            Console.WriteLine("0. Volver");
            Console.Write("\nOpción: ");

            switch (Console.ReadLine())
            {
                case "1": Registrar(service); break;
                case "2": Listar(service); break;
                case "3": Actualizar(service); break;
                case "0": salir = true; break;
                default:
                    Console.WriteLine("Opción inválida.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void Registrar(AeropuertoService service)
    {
        Console.Clear();
        Console.WriteLine("--- Registrar Aeropuerto ---");
        Console.Write("Nombre: ");
        var nombre = Console.ReadLine() ?? "";
        Console.Write("Código IATA (ej: BOG): ");
        var iata = Console.ReadLine() ?? "";
        Console.Write("Ciudad: ");
        var ciudad = Console.ReadLine() ?? "";
        Console.Write("País: ");
        var pais = Console.ReadLine() ?? "";

        service.Registrar(new Aeropuerto
        {
            Nombre = nombre,
            CodigoIATA = iata,
            Ciudad = ciudad,
            Pais = pais
        });

        Console.WriteLine("\n✅ Aeropuerto registrado.");
        Console.ReadKey();
    }

    private static void Listar(AeropuertoService service)
    {
        Console.Clear();
        Console.WriteLine("--- Aeropuertos ---\n");
        var lista = service.ObtenerTodos();

        if (!lista.Any())
            Console.WriteLine("No hay aeropuertos registrados.");
        else
            foreach (var a in lista)
                Console.WriteLine($"[{a.Id}] {a.Nombre} | {a.CodigoIATA} | {a.Ciudad}, {a.Pais}");

        Console.ReadKey();
    }

    private static void Actualizar(AeropuertoService service)
    {
        Listar(service);
        Console.Write("ID a actualizar: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        var aeropuerto = service.ObtenerPorId(id);
        if (aeropuerto == null) { Console.WriteLine("No encontrado."); Console.ReadKey(); return; }

        Console.Write($"Nuevo nombre ({aeropuerto.Nombre}): ");
        var nombre = Console.ReadLine();
        Console.Write($"Nueva ciudad ({aeropuerto.Ciudad}): ");
        var ciudad = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(nombre)) aeropuerto.Nombre = nombre;
        if (!string.IsNullOrWhiteSpace(ciudad)) aeropuerto.Ciudad = ciudad;

        service.Actualizar(aeropuerto);
        Console.WriteLine("\n✅ Actualizado correctamente.");
        Console.ReadKey();
    }
}