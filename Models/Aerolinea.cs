namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;

public class Aerolinea
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;
    public bool Activa { get; set; } = true;

    // Navegación
    public ICollection<Vuelo> Vuelos { get; set; } = new List<Vuelo>();
}