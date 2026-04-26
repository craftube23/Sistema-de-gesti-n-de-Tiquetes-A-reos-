using Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Services;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.UI;

public static class MenuReservas
{
    public static void Mostrar()
    {
        var context = DbContextFactory.Create();
        var service = new ReservaService(context);
        var clienteService = new ClienteService(context);
        var vueloService = new VueloService(context);

        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("========== RESERVAS ==========");
            Console.WriteLine("1. Crear reserva");
            Console.WriteLine("2. Listar todas las reservas");
            Console.WriteLine("3. Confirmar reserva");
            Console.WriteLine("4. Cancelar reserva");
            Console.WriteLine("5. Ver reservas de un cliente");
            Console.WriteLine("0. Volver");
            Console.Write("\nOpción: ");

            switch (Console.ReadLine())
            {
                case "1": Crear(service, clienteService, vueloService); break;
                case "2": Listar(service.ObtenerTodas()); break;
                case "3": Confirmar(service); break;
                case "4": Cancelar(service); break;
                case "5": PorCliente(service, clienteService); break;
                case "0": salir = true; break;
                default:
                    Console.WriteLine("Opción inválida.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void Crear(ReservaService service,
        ClienteService clienteService, VueloService vueloService)
    {
        Console.Clear();
        Console.WriteLine("--- Crear Reserva ---\n");

        var clientes = clienteService.ObtenerTodos();
            Console.WriteLine("Clientes:");
            foreach (var c in clientes)
                Console.WriteLine($"  [{c.Id}] {c.Nombres} {c.Apellidos} | {c.NumeroDocumento}");
            Console.Write("ID cliente: ");
            if (!int.TryParse(Console.ReadLine(), out int clienteId)) return;

            var vuelos = vueloService.ObtenerDisponibles();

    if (!vuelos.Any())
    {
        Console.WriteLine("❌ No hay vuelos disponibles en este momento.");
        Console.ReadKey();
        return;
    }

    Console.WriteLine("\nVuelos disponibles:");
    foreach (var v in vuelos)
        Console.WriteLine($"  [{v.Id}] {v.CodigoVuelo} | " +
            $"{v.AeropuertoOrigen.CodigoIATA} → {v.AeropuertoDestino.CodigoIATA} | " +
            $"{v.FechaSalida:yyyy-MM-dd HH:mm} | " +
            $"Asientos libres: {v.AsientosDisponibles} | " +
            $"Precio base: ${v.PrecioPorAsiento:N0}");

    Console.Write("ID vuelo: ");
    if (!int.TryParse(Console.ReadLine(), out int vueloId)) return;

    // Validar que el ID ingresado exista en la lista mostrada
    var vueloSeleccionado = vuelos.FirstOrDefault(v => v.Id == vueloId);
    if (vueloSeleccionado == null)
    {
        Console.WriteLine("❌ El ID ingresado no corresponde a un vuelo disponible.");
        Console.ReadKey();
        return;
    }

        // Mostrar mapa
        var context = DbContextFactory.Create();
        var asientoService = new AsientoService(context);
        var todosAsientos = asientoService.ObtenerPorVuelo(vueloId);

        Console.Clear();
        Console.WriteLine("Mapa de asientos:\n");
        MapaAsientos.Mostrar(todosAsientos);

        // Seleccionar clase
        var clases = asientoService.ObtenerClases();
        Console.WriteLine("\nClases disponibles:");
        foreach (var cl in clases)
            Console.WriteLine($"  [{cl.Id}] {cl.Nombre} — multiplicador x{cl.Multiplicador}");
        Console.Write("ID clase: ");
        if (!int.TryParse(Console.ReadLine(), out int claseId)) return;

        // Precio dinámico según clase
        decimal precioPorAsiento = asientoService.CalcularPrecio(vueloId, claseId);
        Console.WriteLine($"\n  Precio por asiento en esta clase: ${precioPorAsiento:N0}");

        // Selección manual de asientos
        var disponibles = asientoService.ObtenerDisponiblesPorClase(vueloId, claseId);
        if (!disponibles.Any())
        {
            Console.WriteLine("❌ No hay asientos disponibles en esa clase.");
            Console.ReadKey();
            return;
        }

        var asientoIds = MapaAsientos.SeleccionarAsientos(disponibles);
        if (!asientoIds.Any())
        {
            Console.WriteLine("❌ No seleccionaste ningún asiento.");
            Console.ReadKey();
            return;
        }

        try
        {
            // Reservar asientos seleccionados
            asientoService.ReservarAsientos(asientoIds);

            // Crear reserva
            service.Crear(new Reserva
            {
                ClienteId        = clienteId,
                VueloId          = vueloId,
                CantidadAsientos = asientoIds.Count,
                ValorTotal       = precioPorAsiento * asientoIds.Count
            });

            Console.WriteLine($"\n✅ Reserva creada. {asientoIds.Count} asiento(s) reservado(s).");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
        }

        Console.ReadKey();
    }

    private static void Listar(List<Reserva> reservas)
    {
        Console.Clear();
        Console.WriteLine("--- Reservas ---\n");

        if (!reservas.Any())
        {
            Console.WriteLine("No hay reservas registradas.");
            Console.ReadKey();
            return;
        }

        foreach (var r in reservas)
            Console.WriteLine($"[{r.Id}] {r.CodigoReserva} | " +
                $"{r.Cliente.Nombres} {r.Cliente.Apellidos} | " +
                $"{r.Vuelo.AeropuertoOrigen.CodigoIATA} → {r.Vuelo.AeropuertoDestino.CodigoIATA} | " +
                $"Asientos: {r.CantidadAsientos} | Total: ${r.ValorTotal:N0} | {r.EstadoReserva}");

        Console.ReadKey();
    }

    private static void Confirmar(ReservaService service)
    {
        Listar(service.ObtenerTodas());
        Console.Write("ID reserva a confirmar: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        try
        {
            service.Confirmar(id);
            Console.WriteLine("\n✅ Reserva confirmada.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
        }

        Console.ReadKey();
    }

    private static void Cancelar(ReservaService service)
    {
        Listar(service.ObtenerTodas());
        Console.Write("ID reserva a cancelar: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        try
        {
            service.Cancelar(id);
            Console.WriteLine("\n✅ Reserva cancelada. Asientos devueltos al vuelo.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
        }

        Console.ReadKey();
    }

    private static void PorCliente(ReservaService service, ClienteService clienteService)
    {
        Console.Clear();
        Console.Write("ID del cliente: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        var cliente = clienteService.ObtenerPorId(id);
        if (cliente == null)
        {
            Console.WriteLine("❌ Cliente no encontrado.");
            Console.ReadKey();
            return;
        }

        var reservas = service.ObtenerPorCliente(id);
        Console.WriteLine($"\nReservas de {cliente.Nombres} {cliente.Apellidos}:\n");

        if (!reservas.Any())
            Console.WriteLine("No tiene reservas.");
        else
            foreach (var r in reservas)
                Console.WriteLine($"[{r.Id}] {r.CodigoReserva} | " +
                    $"{r.Vuelo.AeropuertoOrigen.CodigoIATA} → " +
                    $"{r.Vuelo.AeropuertoDestino.CodigoIATA} | " +
                    $"{r.EstadoReserva} | ${r.ValorTotal:N0}");

        Console.ReadKey();
    }
}