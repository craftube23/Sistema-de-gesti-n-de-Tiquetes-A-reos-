namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;

public class Cliente
{
    public int Id { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string NumeroDocumento { get; set; } = string.Empty;
    public string TipoDocumento { get; set; } = string.Empty; // CC, PASAPORTE, CE
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;

    // Navegación
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}