namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;

public class Asiento
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public string Estado { get; set; } = "DISPONIBLE"; // DISPONIBLE, RESERVADO, OCUPADO, BLOQUEADO

    public int VueloId { get; set; }
    public int ClaseVueloId { get; set; }

    public Vuelo Vuelo { get; set; } = null!;
    public ClaseVuelo ClaseVuelo { get; set; } = null!;
}