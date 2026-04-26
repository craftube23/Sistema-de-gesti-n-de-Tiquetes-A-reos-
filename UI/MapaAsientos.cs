using Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.UI;

public static class MapaAsientos
{
    public static void Mostrar(List<Asiento> asientos)
    {
        if (!asientos.Any())
        {
            Console.WriteLine("  Este vuelo no tiene asientos generados.");
            return;
        }

        var porClase = asientos.GroupBy(a => a.ClaseVuelo.Nombre).ToList();

        foreach (var grupo in porClase)
        {
            Console.WriteLine();
            Console.ForegroundColor = grupo.Key switch
            {
                "Primera Clase" => ConsoleColor.Cyan,
                "Ejecutiva"     => ConsoleColor.Magenta,
                _               => ConsoleColor.White
            };
            Console.WriteLine($"  ═══ {grupo.Key} ═══");
            Console.ResetColor();

            int porFila = grupo.Key switch
            {
                "Primera Clase" => 3,
                "Ejecutiva"     => 4,
                _               => 6
            };

            int col = 0;
            Console.Write("  ");

            foreach (var asiento in grupo.OrderBy(a => a.Numero))
            {
                string etiqueta = $"{asiento.Numero,-3}";

                (string simbolo, ConsoleColor color) = asiento.Estado switch
                {
                    "DISPONIBLE" => ($"[{etiqueta}]", ConsoleColor.Green),
                    "RESERVADO"  => ($"[{etiqueta}]", ConsoleColor.Yellow),
                    "OCUPADO"    => ($"[XXX]", ConsoleColor.Red),
                    "BLOQUEADO"  => ($"[---]", ConsoleColor.DarkGray),
                    _            => ($"[???]", ConsoleColor.White)
                };

                Console.ForegroundColor = color;
                Console.Write(simbolo + " ");
                Console.ResetColor();

                col++;
                if (col % porFila == 0)
                {
                    Console.WriteLine();
                    Console.Write("  ");
                }
            }

            Console.WriteLine();
        }

        // Leyenda
        Console.WriteLine();
        Console.Write("  Leyenda: ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("[1A ] Disponible  ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("[1A ] Reservado  ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("[XXX] Ocupado  ");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("[---] Bloqueado");
        Console.ResetColor();
        Console.WriteLine();
    }

    public static List<int> SeleccionarAsientos(List<Asiento> disponibles)
    {
        var seleccionados = new List<int>();

        Console.WriteLine("\n  Asientos disponibles:");
        foreach (var a in disponibles)
            Console.WriteLine($"    [{a.Id}] {a.Numero} — {a.ClaseVuelo.Nombre}");

        Console.WriteLine("\n  Ingresa los IDs de los asientos uno por uno.");
        Console.WriteLine("  Escribe 0 cuando termines.");

        while (true)
        {
            Console.Write("  ID asiento: ");
            var input = Console.ReadLine();

            if (input == "0") break;

            if (!int.TryParse(input, out int id))
            {
                Console.WriteLine("  ❌ ID inválido.");
                continue;
            }

            var asiento = disponibles.FirstOrDefault(a => a.Id == id);
            if (asiento == null)
            {
                Console.WriteLine("  ❌ Ese asiento no está en la lista de disponibles.");
                continue;
            }

            if (seleccionados.Contains(id))
            {
                Console.WriteLine("  ⚠️  Ya seleccionaste ese asiento.");
                continue;
            }

            seleccionados.Add(id);
            Console.WriteLine($"  ✅ Asiento {asiento.Numero} agregado.");
        }

        return seleccionados;
    }
}