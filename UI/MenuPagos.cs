using Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Services;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.UI;

public static class MenuPagos
{
    public static void Mostrar()
    {
        var context = DbContextFactory.Create();
        var pagoService = new PagoService(context);
        var reservaService = new ReservaService(context);

        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("========== PAGOS ==========");
            Console.WriteLine("1. Registrar pago de reserva");
            Console.WriteLine("2. Listar todos los pagos");
            Console.WriteLine("3. Cambiar estado de pago");
            Console.WriteLine("0. Volver");
            Console.Write("\nOpción: ");

            switch (Console.ReadLine())
            {
                case "1": Registrar(pagoService, reservaService); break;
                case "2": Listar(pagoService); break;
                case "3": CambiarEstado(pagoService); break;
                case "0": salir = true; break;
                default:
                    Console.WriteLine("Opción inválida.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void Registrar(PagoService pagoService,
        ReservaService reservaService)
    {
        Console.Clear();
        Console.WriteLine("--- Registrar Pago ---\n");

        var reservas = reservaService.ObtenerTodas()
            .Where(r => r.EstadoReserva != "CANCELADA" && r.Pago == null)
            .ToList();

        if (!reservas.Any())
        {
            Console.WriteLine("No hay reservas pendientes de pago.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Reservas sin pago:");
        foreach (var r in reservas)
            Console.WriteLine($"  [{r.Id}] {r.CodigoReserva} | " +
                $"{r.Cliente.Nombres} {r.Cliente.Apellidos} | " +
                $"${r.ValorTotal:N0} | {r.EstadoReserva}");

        Console.Write("\nID de la reserva: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        var reserva = reservas.FirstOrDefault(r => r.Id == id);
        if (reserva == null)
        {
            Console.WriteLine("❌ Reserva no encontrada en la lista.");
            Console.ReadKey();
            return;
        }

        // Método de pago con opciones numeradas
        Console.WriteLine("\nMétodo de pago:");
        Console.WriteLine("  [1] Tarjeta");
        Console.WriteLine("  [2] Efectivo");
        Console.WriteLine("  [3] PSE");
        Console.Write("Opción: ");

        var metodo = Console.ReadLine() switch
        {
            "1" => "TARJETA",
            "2" => "EFECTIVO",
            "3" => "PSE",
            _   => ""
        };

        if (string.IsNullOrEmpty(metodo))
        {
            Console.WriteLine("❌ Método de pago inválido.");
            Console.ReadKey();
            return;
        }

        try
        {
            pagoService.Registrar(id, metodo);
            Console.WriteLine($"\n✅ Pago de ${reserva.ValorTotal:N0} registrado con {metodo}.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
        }

        Console.ReadKey();
    }
    private static void Listar(PagoService service)
    {
        Console.Clear();
        Console.WriteLine("--- Pagos registrados ---\n");
        var lista = service.ObtenerTodos();

        if (!lista.Any())
        {
            Console.WriteLine("No hay pagos registrados.");
            Console.ReadKey();
            return;
        }

        foreach (var p in lista)
            Console.WriteLine($"[{p.Id}] {p.Reserva.CodigoReserva} | " +
                $"{p.Reserva.Cliente.Nombres} {p.Reserva.Cliente.Apellidos} | " +
                $"${p.Monto:N0} | {p.MetodoPago} | {p.Estado} | " +
                $"{p.FechaPago:yyyy-MM-dd HH:mm}");

        Console.ReadKey();
    }

    private static void CambiarEstado(PagoService service)
    {
        Listar(service);
        Console.Write("ID del pago: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        Console.Write("Nuevo estado (PENDIENTE / PAGADO / RECHAZADO): ");
        var estado = Console.ReadLine() ?? "";

        try
        {
            service.CambiarEstado(id, estado);
            Console.WriteLine("\n✅ Estado actualizado.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
        }

        Console.ReadKey();
    }
}