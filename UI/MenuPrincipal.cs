using Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.UI;

public static class MenuPrincipal
{
    public static void Mostrar()
    {
        // Verificar conexión a base de datos al iniciar
        try
        {
            using var context = DbContextFactory.Create();
            context.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Error al conectar con la base de datos:");
            Console.WriteLine(ex.Message);
            Console.ResetColor();
            Console.ReadKey();
            return;
        }

        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║    SISTEMA DE GESTIÓN DE TIQUETES AÉREOS     ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("  1. ✈  Aerolíneas");
            Console.WriteLine("  2. 🏢  Aeropuertos");
            Console.WriteLine("  3. 🛫  Vuelos");
            Console.WriteLine("  4. 👤  Clientes");
            Console.WriteLine("  5. 📋  Reservas");
            Console.WriteLine("  6. 🎫  Tiquetes");
            Console.WriteLine("  7. 💳  Pagos");
            Console.WriteLine("  8. 📊  Reportes");
            Console.WriteLine();
            Console.WriteLine("  0. 🚪  Salir");
            Console.WriteLine();
            Console.Write("  Opción: ");

            switch (Console.ReadLine())
            {
                case "1": MenuAerolineas.Mostrar(); break;
                case "2": MenuAeropuertos.Mostrar(); break;
                case "3": MenuVuelos.Mostrar(); break;
                case "4": MenuClientes.Mostrar(); break;
                case "5": MenuReservas.Mostrar(); break;
                case "6": MenuTiquetes.Mostrar(); break;
                case "7": MenuPagos.Mostrar(); break;
                case "8": MenuReportes.Mostrar(); break;
                case "0":
                    salir = true;
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Hasta luego. ✈");
                    Console.ResetColor();
                    break;
                default:
                    Console.WriteLine("  Opción inválida.");
                    Console.ReadKey();
                    break;
            }
        }
    }
}