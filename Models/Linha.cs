
namespace OndeALinhaDesligouWeb.Models;

public class Linha
{
    public required string Nome { get; set; }
    public required string Chave { get; set; }
    public required string ColunaA { get; set; }
    public required string ColunaB { get; set; }
    public required string ArquivoExcel { get; set; }
    public required string Grupo { get; set; } // "Londrina" ou "Campo Mourão"
}
