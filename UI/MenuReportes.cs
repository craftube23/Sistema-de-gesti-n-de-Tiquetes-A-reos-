using Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;
using Sistema_de_gesti_n_de_Tiquetes_Areos_.Services;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.UI;

public static class MenuReportes
{
    public static void Mostrar()
    {
        var context = DbContextFactory.Create();
        var service = new ReporteService(context);

        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("========== REPORTES ==========");
            Console.WriteLine("1. Vuelos disponibles");
            Console.WriteLine("2. Destinos más solicitados");
            Console.WriteLine("3. Clientes frecuentes");
            Console.WriteLine("4. Ingresos por aerolínea");
            Console.WriteLine("5. Reservas por estado");
            Console.WriteLine("6. Vuelos con mayor ocupación");
            Console.WriteLine("7. Tiquetes por rango de fechas");
            Console.WriteLine("0. Volver");
            Console.Write("\nOpción: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Clear();
                    service.VuelosDisponibles();
                    Console.ReadKey();
                    break;
                case "2":
                    Console.Clear();
                    service.DestinosMasSolicitados();
                    Console.ReadKey();
                    break;
                case "3":
                    Console.Clear();
                    service.ClientesFrecuentes();
                    Console.ReadKey();
                    break;
                case "4":
                    Console.Clear();
                    service.IngresosPorAerolinea();
                    Console.ReadKey();
                    break;
                case "5":
                    Console.Clear();
                    service.ReservasPorEstado();
                    Console.ReadKey();
                    break;
                case "6":
                    Console.Clear();
                    service.VuelosMayorOcupacion();
                    Console.ReadKey();
                    break;
                case "7":
                    Console.Clear();
                    Console.Write("Fecha desde (yyyy-MM-dd): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime desde)) break;
                    Console.Write("Fecha hasta (yyyy-MM-dd): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime hasta)) break;
                    Console.Clear();
                    service.TiquetesPorRangoFechas(desde, hasta);
                    Console.ReadKey();
                    break;
                case "0":
                    salir = true;
                    break;
                default:
                    Console.WriteLine("Opción inválida.");
                    Console.ReadKey();
                    break;
            }
        }
    }
}