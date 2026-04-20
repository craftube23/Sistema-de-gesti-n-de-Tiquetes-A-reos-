using Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Services;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.UI;

public static class MenuAerolineas
{
    public static void Mostrar()
    {
        var context = DbContextFactory.Create();
        var service = new AerolineaService(context);

        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("========== AEROLÍNEAS ==========");
            Console.WriteLine("1. Registrar aerolínea");
            Console.WriteLine("2. Listar aerolíneas");
            Console.WriteLine("3. Actualizar aerolínea");
            Console.WriteLine("4. Desactivar aerolínea");
            Console.WriteLine("0. Volver");
            Console.Write("\nOpción: ");

            switch (Console.ReadLine())
            {
                case "1": Registrar(service); break;
                case "2": Listar(service); break;
                case "3": Actualizar(service); break;
                case "4": Desactivar(service); break;
                case "0": salir = true; break;
                default:
                    Console.WriteLine("Opción inválida.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void Registrar(AerolineaService service)
    {
        Console.Clear();
        Console.WriteLine("--- Registrar Aerolínea ---");
        Console.Write("Nombre: ");
        var nombre = Console.ReadLine() ?? "";
        Console.Write("Código (ej: AV): ");
        var codigo = Console.ReadLine() ?? "";
        Console.Write("País: ");
        var pais = Console.ReadLine() ?? "";

        service.Registrar(new Aerolinea
        {
            Nombre = nombre,
            Codigo = codigo,
            Pais = pais
        });

        Console.WriteLine("\n✅ Aerolínea registrada correctamente.");
        Console.ReadKey();
    }

    private static void Listar(AerolineaService service)
    {
        Console.Clear();
        Console.WriteLine("--- Aerolíneas activas ---\n");
        var lista = service.ObtenerTodas();

        if (!lista.Any())
        {
            Console.WriteLine("No hay aerolíneas registradas.");
        }
        else
        {
            foreach (var a in lista)
                Console.WriteLine($"[{a.Id}] {a.Nombre} | {a.Codigo} | {a.Pais}");
        }

        Console.ReadKey();
    }

    private static void Actualizar(AerolineaService service)
    {
        Listar(service);
        Console.Write("ID a actualizar: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        var aerolinea = service.ObtenerPorId(id);
        if (aerolinea == null) { Console.WriteLine("No encontrada."); Console.ReadKey(); return; }

        Console.Write($"Nuevo nombre ({aerolinea.Nombre}): ");
        var nombre = Console.ReadLine();
        Console.Write($"Nuevo código ({aerolinea.Codigo}): ");
        var codigo = Console.ReadLine();
        Console.Write($"Nuevo país ({aerolinea.Pais}): ");
        var pais = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(nombre)) aerolinea.Nombre = nombre;
        if (!string.IsNullOrWhiteSpace(codigo)) aerolinea.Codigo = codigo;
        if (!string.IsNullOrWhiteSpace(pais)) aerolinea.Pais = pais;

        service.Actualizar(aerolinea);
        Console.WriteLine("\n✅ Actualizada correctamente.");
        Console.ReadKey();
    }

    private static void Desactivar(AerolineaService service)
    {
        Listar(service);
        Console.Write("ID a desactivar: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        service.Desactivar(id);
        Console.WriteLine("\n✅ Aerolínea desactivada.");
        Console.ReadKey();
    }
}