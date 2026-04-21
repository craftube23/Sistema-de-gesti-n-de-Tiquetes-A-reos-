using Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Services;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.UI;

public static class MenuTiquetes
{
    public static void Mostrar()
    {
        var context = DbContextFactory.Create();
        var tiqueteService = new TiqueteService(context);
        var reservaService = new ReservaService(context);

        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("========== TIQUETES ==========");
            Console.WriteLine("1. Emitir tiquete desde reserva confirmada");
            Console.WriteLine("2. Listar todos los tiquetes");
            Console.WriteLine("3. Ver detalle de tiquete");
            Console.WriteLine("4. Cambiar estado de tiquete");
            Console.WriteLine("0. Volver");
            Console.Write("\nOpción: ");

            switch (Console.ReadLine())
            {
                case "1": Emitir(tiqueteService, reservaService); break;
                case "2": Listar(tiqueteService); break;
                case "3": Detalle(tiqueteService); break;
                case "4": CambiarEstado(tiqueteService); break;
                case "0": salir = true; break;
                default:
                    Console.WriteLine("Opción inválida.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void Emitir(TiqueteService tiqueteService,
        ReservaService reservaService)
    {
        Console.Clear();
        Console.WriteLine("--- Emitir Tiquete ---\n");

        // Mostrar solo reservas confirmadas
        var reservas = reservaService.ObtenerTodas()
            .Where(r => r.EstadoReserva == "CONFIRMADA")
            .ToList();

        if (!reservas.Any())
        {
            Console.WriteLine("No hay reservas confirmadas disponibles.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Reservas confirmadas:");
        foreach (var r in reservas)
            Console.WriteLine($"  [{r.Id}] {r.CodigoReserva} | " +
                $"{r.Cliente.Nombres} {r.Cliente.Apellidos} | " +
                $"{r.Vuelo.AeropuertoOrigen.CodigoIATA} → " +
                $"{r.Vuelo.AeropuertoDestino.CodigoIATA} | " +
                $"${r.ValorTotal:N0}");

        Console.Write("\nID de la reserva: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        try
        {
            tiqueteService.Emitir(id);
            Console.WriteLine("\n✅ Tiquete emitido correctamente.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
        }

        Console.ReadKey();
    }

    private static void Listar(TiqueteService service)
    {
        Console.Clear();
        Console.WriteLine("--- Tiquetes emitidos ---\n");
        var lista = service.ObtenerTodos();

        if (!lista.Any())
        {
            Console.WriteLine("No hay tiquetes emitidos.");
            Console.ReadKey();
            return;
        }

        foreach (var t in lista)
            Console.WriteLine($"[{t.Id}] {t.CodigoTiquete} | " +
                $"{t.Reserva.Cliente.Nombres} {t.Reserva.Cliente.Apellidos} | " +
                $"{t.Reserva.Vuelo.AeropuertoOrigen.CodigoIATA} → " +
                $"{t.Reserva.Vuelo.AeropuertoDestino.CodigoIATA} | " +
                $"{t.FechaEmision:yyyy-MM-dd} | {t.Estado}");

        Console.ReadKey();
    }

    private static void Detalle(TiqueteService service)
    {
        Console.Clear();
        Console.Write("ID del tiquete: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        var t = service.ObtenerPorId(id);
        if (t == null)
        {
            Console.WriteLine("❌ Tiquete no encontrado.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\n{'=',40}");
        Console.WriteLine($"  TIQUETE: {t.CodigoTiquete}");
        Console.WriteLine($"{'=',40}");
        Console.WriteLine($"  Pasajero : {t.Reserva.Cliente.Nombres} {t.Reserva.Cliente.Apellidos}");
        Console.WriteLine($"  Documento: {t.Reserva.Cliente.TipoDocumento} {t.Reserva.Cliente.NumeroDocumento}");
        Console.WriteLine($"  Vuelo    : {t.Reserva.Vuelo.CodigoVuelo}");
        Console.WriteLine($"  Ruta     : {t.Reserva.Vuelo.AeropuertoOrigen.CodigoIATA} → {t.Reserva.Vuelo.AeropuertoDestino.CodigoIATA}");
        Console.WriteLine($"  Salida   : {t.Reserva.Vuelo.FechaSalida:yyyy-MM-dd HH:mm}");
        Console.WriteLine($"  Asientos : {t.Reserva.CantidadAsientos}");
        Console.WriteLine($"  Total    : ${t.Reserva.ValorTotal:N0}");
        Console.WriteLine($"  Emitido  : {t.FechaEmision:yyyy-MM-dd HH:mm}");
        Console.WriteLine($"  Estado   : {t.Estado}");
        Console.WriteLine($"{'=',40}");

        Console.ReadKey();
    }

    private static void CambiarEstado(TiqueteService service)
    {
        Listar(service);
        Console.Write("ID del tiquete: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        Console.Write("Nuevo estado (EMITIDO / USADO / ANULADO): ");
        var estado = Console.ReadLine() ?? "";

        try
        {
            service.CambiarEstado(id, estado.ToUpper());
            Console.WriteLine("\n✅ Estado actualizado.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
        }

        Console.ReadKey();
    }
}