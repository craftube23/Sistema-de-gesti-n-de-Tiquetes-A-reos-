namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Models;

public class ClaseVuelo
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Multiplicador { get; set; } = 1.0m;

    public ICollection<Asiento> Asientos { get; set; } = new List<Asiento>();
}