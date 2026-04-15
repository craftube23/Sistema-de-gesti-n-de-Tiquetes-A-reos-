namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;

public class Aeropuerto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string CodigoIATA { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;

    // Navegación
    public ICollection<Vuelo> VuelosOrigen { get; set; } = new List<Vuelo>();
    public ICollection<Vuelo> VuelosDestino { get; set; } = new List<Vuelo>();
}