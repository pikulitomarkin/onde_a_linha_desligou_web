using OndeALinhaDesligouWeb.Models;

namespace OndeALinhaDesligouWeb.Options;

public class LinhasOptions
{
    public required string ResourcesPath { get; set; }
    public required List<Linha> Linhas { get; set; }
    public required Dictionary<string, string> LinhasGpx { get; set; }
}
