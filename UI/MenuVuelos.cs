using Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Services;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.UI;

public static class MenuVuelos
{
    public static void Mostrar()
    {
        var context = DbContextFactory.Create();
        var vueloService = new VueloService(context);
        var aerolineaService = new AerolineaService(context);
        var aeropuertoService = new AeropuertoService(context);

        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("========== VUELOS ==========");
            Console.WriteLine("1. Registrar vuelo");
            Console.WriteLine("2. Listar todos los vuelos");
            Console.WriteLine("3. Listar vuelos disponibles");
            Console.WriteLine("4. Cambiar estado de vuelo");
            Console.WriteLine("0. Volver");
            Console.Write("\nOpción: ");

            switch (Console.ReadLine())
            {
                case "1": Registrar(vueloService, aerolineaService, aeropuertoService); break;
                case "2": Listar(vueloService.ObtenerTodos()); break;
                case "3": Listar(vueloService.ObtenerDisponibles()); break;
                case "4": CambiarEstado(vueloService); break;
                case "0": salir = true; break;
                default:
                    Console.WriteLine("Opción inválida.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void Registrar(VueloService vueloService,
        AerolineaService aerolineaService, AeropuertoService aeropuertoService)
    {
        Console.Clear();
        Console.WriteLine("--- Registrar Vuelo ---\n");

        Console.Write("Código de vuelo (ej: AV101): ");
        var codigo = Console.ReadLine() ?? "";

        // Seleccionar aerolínea
        var aerolineas = aerolineaService.ObtenerTodas();
        Console.WriteLine("\nAerolíneas:");
        foreach (var a in aerolineas)
            Console.WriteLine($"  [{a.Id}] {a.Nombre}");
        Console.Write("ID aerolínea: ");
        if (!int.TryParse(Console.ReadLine(), out int aerolineaId)) return;

        // Seleccionar aeropuertos
        var aeropuertos = aeropuertoService.ObtenerTodos();
        Console.WriteLine("\nAeropuertos:");
        foreach (var a in aeropuertos)
            Console.WriteLine($"  [{a.Id}] {a.Nombre} ({a.CodigoIATA}) - {a.Ciudad}");

        Console.Write("ID aeropuerto origen: ");
        if (!int.TryParse(Console.ReadLine(), out int origenId)) return;
        Console.Write("ID aeropuerto destino: ");
        if (!int.TryParse(Console.ReadLine(), out int destinoId)) return;

        Console.Write("Fecha y hora salida (yyyy-MM-dd HH:mm): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime salida)) return;
        Console.Write("Fecha y hora llegada (yyyy-MM-dd HH:mm): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime llegada)) return;

        Console.Write("Capacidad total: ");
        if (!int.TryParse(Console.ReadLine(), out int capacidad)) return;
        Console.Write("Precio por asiento: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal precio)) return;

        vueloService.Registrar(new Vuelo
        {
            CodigoVuelo = codigo,
            AerolineaId = aerolineaId,
            AeropuertoOrigenId = origenId,
            AeropuertoDestinoId = destinoId,
            FechaSalida = salida,
            FechaLlegada = llegada,
            CapacidadTotal = capacidad,
            PrecioPorAsiento = precio
        });

        Console.WriteLine("\n✅ Vuelo registrado correctamente.");
        Console.ReadKey();
    }

    private static void Listar(List<Vuelo> vuelos)
    {
        Console.Clear();
        Console.WriteLine("--- Vuelos ---\n");

        if (!vuelos.Any())
        {
            Console.WriteLine("No hay vuelos.");
            Console.ReadKey();
            return;
        }

        foreach (var v in vuelos)
            Console.WriteLine($"[{v.Id}] {v.CodigoVuelo} | {v.AeropuertoOrigen.CodigoIATA} → " +
                $"{v.AeropuertoDestino.CodigoIATA} | {v.FechaSalida:yyyy-MM-dd HH:mm} | " +
                $"{v.Aerolinea.Nombre} | Asientos: {v.AsientosDisponibles}/{v.CapacidadTotal} | {v.Estado}");

        Console.ReadKey();
    }

    private static void CambiarEstado(VueloService service)
    {
        Listar(service.ObtenerTodos());
        Console.Write("ID vuelo: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;
        Console.Write("Nuevo estado (PROGRAMADO / CANCELADO / COMPLETADO): ");
        var estado = Console.ReadLine() ?? "";
        service.ActualizarEstado(id, estado.ToUpper());
        Console.WriteLine("\n✅ Estado actualizado.");
        Console.ReadKey();
    }
}