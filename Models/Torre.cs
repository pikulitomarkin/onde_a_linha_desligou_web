
namespace OndeALinhaDesligouWeb.Models;

public class Torre
{
    public required string CodigoOriginal { get; set; }
    public required string NumeroParaExibicao { get; set; }
    public required string Cidade { get; set; }
    public required string Setor { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
