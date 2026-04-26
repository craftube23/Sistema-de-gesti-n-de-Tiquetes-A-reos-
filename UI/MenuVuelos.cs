using Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Services;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.UI;

public static class MenuVuelos
{
    public static void Mostrar()
    {
        var context = DbContextFactory.Create();
        var asientoService = new AsientoService(context);
        var vueloService = new VueloService(context);
        var aerolineaService = new AerolineaService(context);
        var aeropuertoService = new AeropuertoService(context);

        bool salir = false;
        while (!salir)
        {
            Console.WriteLine("========== VUELOS ==========");
            Console.WriteLine("1. Registrar vuelo");
            Console.WriteLine("2. Listar todos los vuelos");
            Console.WriteLine("3. Listar vuelos disponibles");
            Console.WriteLine("4. Cambiar estado de vuelo");
            Console.WriteLine("5. Ver mapa de asientos");
            Console.WriteLine("0. Volver");
            Console.Write("\nOpción: ");

            switch (Console.ReadLine())
            {
                case "1": Registrar(vueloService, aerolineaService, aeropuertoService); break;
                case "2": Listar(vueloService.ObtenerTodos()); break;
                case "3": Listar(vueloService.ObtenerDisponibles()); break;
                case "4": CambiarEstado(vueloService); break;
                case "5": VerMapa(vueloService, context); break;
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
        if (!aerolineas.Any())
        {
            Console.WriteLine("❌ No hay aerolíneas registradas. Registra una primero.");
            Console.ReadKey();
            return;
        }
        Console.WriteLine("\nAerolíneas:");
        foreach (var a in aerolineas)
            Console.WriteLine($"  [{a.Id}] {a.Nombre}");
        Console.Write("ID aerolínea: ");
        if (!int.TryParse(Console.ReadLine(), out int aerolineaId)) return;

        // Seleccionar aeropuertos
        var aeropuertos = aeropuertoService.ObtenerTodos();
        if (!aeropuertos.Any())
        {
            Console.WriteLine("❌ No hay aeropuertos registrados. Registra uno primero.");
            Console.ReadKey();
            return;
        }
        Console.WriteLine("\nAeropuertos:");
        foreach (var a in aeropuertos)
            Console.WriteLine($"  [{a.Id}] {a.Nombre} ({a.CodigoIATA}) - {a.Ciudad}");

        Console.Write("ID aeropuerto origen: ");
        if (!int.TryParse(Console.ReadLine(), out int origenId)) return;
        Console.Write("ID aeropuerto destino: ");
        if (!int.TryParse(Console.ReadLine(), out int destinoId)) return;

        if (origenId == destinoId)
        {
            Console.WriteLine("❌ El origen y destino no pueden ser el mismo aeropuerto.");
            Console.ReadKey();
            return;
        }

        Console.Write("Fecha y hora salida (yyyy-MM-dd HH:mm): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime salida)) return;
        Console.Write("Fecha y hora llegada (yyyy-MM-dd HH:mm): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime llegada)) return;

        if (llegada <= salida)
        {
            Console.WriteLine("❌ La hora de llegada debe ser posterior a la salida.");
            Console.ReadKey();
            return;
        }

        // Distribución de asientos por clase
        Console.WriteLine("\n--- Distribución de asientos por clase ---");

        Console.Write("Asientos Primera Clase:  ");
        if (!int.TryParse(Console.ReadLine(), out int primera) || primera < 0) return;

        Console.Write("Asientos Ejecutiva:      ");
        if (!int.TryParse(Console.ReadLine(), out int ejecutiva) || ejecutiva < 0) return;

        Console.Write("Asientos Económica:      ");
        if (!int.TryParse(Console.ReadLine(), out int economica) || economica < 0) return;

        int totalAsientos = primera + ejecutiva + economica;

        if (totalAsientos == 0)
        {
            Console.WriteLine("❌ El vuelo debe tener al menos un asiento.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\n  Primera Clase : {primera}");
        Console.WriteLine($"  Ejecutiva     : {ejecutiva}");
        Console.WriteLine($"  Económica     : {economica}");
        Console.WriteLine($"  Total asientos: {totalAsientos}");

        Console.Write("\nPrecio base por asiento (Económica): $");
        if (!decimal.TryParse(Console.ReadLine(), out decimal precio) || precio <= 0) return;

        Console.WriteLine($"\n  Precio Económica    : ${precio:N0}");
        Console.WriteLine($"  Precio Ejecutiva    : ${precio * 1.8m:N0}  (x1.8)");
        Console.WriteLine($"  Precio Primera Clase: ${precio * 3.0m:N0}  (x3.0)");

        Console.Write("\n¿Confirmar registro? (s/n): ");
        if (Console.ReadLine()?.ToLower() != "s") return;

        var context = DbContextFactory.Create();
        var asientoService = new AsientoService(context);

        var vuelo = new Vuelo
        {
            CodigoVuelo           = codigo,
            AerolineaId           = aerolineaId,
            AeropuertoOrigenId    = origenId,
            AeropuertoDestinoId   = destinoId,
            FechaSalida           = salida,
            FechaLlegada          = llegada,
            AsientosPrimeraClase  = primera,
            AsientosEjecutiva     = ejecutiva,
            AsientosEconomica     = economica,
            CapacidadTotal        = totalAsientos,
            AsientosDisponibles   = totalAsientos,
            PrecioPorAsiento      = precio
        };

        try
        {
            vueloService.Registrar(vuelo);
            // Generar asientos automáticamente al registrar el vuelo
            asientoService.GenerarAsientos(vuelo.Id);
            Console.WriteLine($"\n✅ Vuelo registrado con {totalAsientos} asientos generados.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
        }

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

    private static void VerMapa(VueloService vueloService, AppDbContext context)
    {
        Console.Clear();
        Console.WriteLine("--- Mapa de Asientos ---\n");

        var vuelos = vueloService.ObtenerTodos();
        if (!vuelos.Any())
        {
            Console.WriteLine("No hay vuelos registrados.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Vuelos registrados:");
        foreach (var v in vuelos)
            Console.WriteLine($"  [{v.Id}] {v.CodigoVuelo} | " +
                $"{v.AeropuertoOrigen.CodigoIATA} → {v.AeropuertoDestino.CodigoIATA} | " +
                $"{v.FechaSalida:yyyy-MM-dd HH:mm} | {v.Estado}");

        Console.Write("\nID del vuelo: ");
        if (!int.TryParse(Console.ReadLine(), out int vueloId)) return;

        var asientoService = new AsientoService(context);
        var asientos = asientoService.ObtenerPorVuelo(vueloId);

        Console.Clear();
        MapaAsientos.Mostrar(asientos);
        Console.ReadKey();
    }
        
}