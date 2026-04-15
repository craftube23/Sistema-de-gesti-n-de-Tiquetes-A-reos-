namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;

public class Pago
{
    public int Id { get; set; }
    public decimal Monto { get; set; }
    public string MetodoPago { get; set; } = string.Empty; // TARJETA, EFECTIVO, PSE
    public string Estado { get; set; } = "PENDIENTE"; // PENDIENTE, PAGADO, RECHAZADO
    public DateTime FechaPago { get; set; } = DateTime.Now;

    // Llave foránea
    public int ReservaId { get; set; }

    // Navegación
    public Reserva Reserva { get; set; } = null!;
}