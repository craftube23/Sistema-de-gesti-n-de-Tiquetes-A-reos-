namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;

public class Vuelo
{
    public int Id { get; set; }
    public string CodigoVuelo { get; set; } = string.Empty;
    public DateTime FechaSalida { get; set; }
    public DateTime FechaLlegada { get; set; }
    public int CapacidadTotal { get; set; }
    public int AsientosDisponibles { get; set; }
    public string Estado { get; set; } = "PROGRAMADO"; // PROGRAMADO, CANCELADO, COMPLETADO
    public decimal PrecioPorAsiento { get; set; }

    //Asientos Disponibles
    public int AsientosEconomica { get; set; }
    public int AsientosEjecutiva { get; set; }
    public int AsientosPrimeraClase { get; set; }

    // Llaves foráneas
    public int AerolineaId { get; set; }
    public int AeropuertoOrigenId { get; set; }
    public int AeropuertoDestinoId { get; set; }

    // Navegación
    public Aerolinea Aerolinea { get; set; } = null!;
    public Aeropuerto AeropuertoOrigen { get; set; } = null!;
    public Aeropuerto AeropuertoDestino { get; set; } = null!;
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    public ICollection<Asiento> Asientos { get; set; } = new List<Asiento>();
}
