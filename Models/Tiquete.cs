namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;

public class Tiquete
{
    public int Id { get; set; }
    public string CodigoTiquete { get; set; } = string.Empty;
    public DateTime FechaEmision { get; set; } = DateTime.Now;
    public string Estado { get; set; } = "EMITIDO"; // EMITIDO, USADO, ANULADO

    // Llave foránea
    public int ReservaId { get; set; }

    // Navegación
    public Reserva Reserva { get; set; } = null!;
}