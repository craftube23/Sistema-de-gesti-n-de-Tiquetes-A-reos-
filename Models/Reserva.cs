namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;

public class Reserva
{
    public int Id { get; set; }
    public string CodigoReserva { get; set; } = string.Empty;
    public int CantidadAsientos { get; set; }
    public decimal ValorTotal { get; set; }
    public string EstadoReserva { get; set; } = "PENDIENTE"; // PENDIENTE, CONFIRMADA, CANCELADA
    public DateTime FechaReserva { get; set; } = DateTime.Now;

    // Llaves foráneas
    public int ClienteId { get; set; }
    public int VueloId { get; set; }

    // Navegación
    public Cliente Cliente { get; set; } = null!;
    public Vuelo Vuelo { get; set; } = null!;
    public Tiquete? Tiquete { get; set; }
    public Pago? Pago { get; set; }
}