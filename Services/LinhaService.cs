
using OndeALinhaDesligouWeb.Models;
using OfficeOpenXml;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace OndeALinhaDesligouWeb.Services;

public class LinhaService
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private static readonly List<Linha> _linhas = new()
    {
        // Grupo Londrina
        new Linha { Nome = "Linha Londrina - Londrina Sul", Chave = "lonlns", ColunaA = "KMLON", ColunaB = "KMAPA", ArquivoExcel = "KM LON LNS.xlsx", Grupo = "Londrina" },
        new Linha { Nome = "Linha Londrina - Londrina Copel C1", Chave = "lonlna", ColunaA = "KM - LON - LNA", ColunaB = "KM - LNA - LON", ArquivoExcel = "KM LON LNA.xlsx", Grupo = "Londrina" },
        new Linha { Nome = "Linha Londrina - Londrina Copel C2", Chave = "lonlna2", ColunaA = "LON-LNA", ColunaB = "LNA-LON", ArquivoExcel = "KM LON LNA2.xlsx", Grupo = "Londrina" },
        new Linha { Nome = "Linha Londrina Sul - Apucarana", Chave = "apucarana", ColunaA = "LNS", ColunaB = "APA", ArquivoExcel = "KM LON APA.xlsx", Grupo = "Londrina" },
        new Linha { Nome = "Linha Sarandi - Londrina", Chave = "lon_sdi", ColunaA = "LON-SDI", ColunaB = "SDI-LON", ArquivoExcel = "KM LON SDI.xlsx", Grupo = "Londrina" },
        new Linha { Nome = "Linha Maringa - Sarandi", Chave = "lon_mga", ColunaA = "MGA-SDI", ColunaB = "SDI-MGA", ArquivoExcel = "KM MGA SDI.xlsx", Grupo = "Londrina" },
        new Linha { Nome = "Linha Assis C2 - Londrina Copel", Chave = "assis_c2_londrina_norte", ColunaA = "ASS-LNA", ColunaB = "LNA-ASS", ArquivoExcel = "KM LNA ASS2.xlsx", Grupo = "Londrina" },
        new Linha { Nome = "Linha Assis C1 - Londrina Copel", Chave = "lna_assis", ColunaA = "KM - ASS - LNA", ColunaB = "KM - LNA - ASS", ArquivoExcel = "KM LNA ASS.xlsx", Grupo = "Londrina" },
        new Linha { Nome = "Linha Ivaiporã - Londrina", Chave = "ivp_lon", ColunaA = "KMIVP", ColunaB = "KMLON", ArquivoExcel = "KM IVP LON.xlsx", Grupo = "Londrina" },

        // Grupo Campo Mourão
        new Linha { Nome = "Linha Campo Mourão - Apucarana", Chave = "cmo_apa", ColunaA = "KMCMO", ColunaB = "KMAPA", ArquivoExcel = "KM CMO APA.xlsx", Grupo = "Campo Mourão" },
        new Linha { Nome = "Linha Campo Mourão - Maringá", Chave = "cmo_mga", ColunaA = "KMCMO", ColunaB = "KMMGA", ArquivoExcel = "KM CMO MGA.xlsx", Grupo = "Campo Mourão" },
        new Linha { Nome = "Linha Salto Osório - Campo Mourão", Chave = "cmo_sos", ColunaA = "KMSOS", ColunaB = "KMCMO", ArquivoExcel = "KM CMO SOS.xlsx", Grupo = "Campo Mourão" },
        new Linha { Nome = "Linha Salto Osório C2 - Campo Mourão", Chave = "cmo_sos c2", ColunaA = "KMSOS", ColunaB = "KMCMO", ArquivoExcel = "KM CMO SOSC2.xlsx", Grupo = "Campo Mourão" },
        new Linha { Nome = "Linha Salto Santiago - Campo Mourão", Chave = "cmo_ssa", ColunaA = "KMSSA", ColunaB = "KMIVP", ArquivoExcel = "KM CMO SSA.xlsx", Grupo = "Campo Mourão" },
        new Linha { Nome = "Linha Salto Santiago C2 - Campo Mourão", Chave = "cmo_ssac2", ColunaA = "KMSSA", ColunaB = "KMIVP", ArquivoExcel = "KM CMO SSAC2.xlsx", Grupo = "Campo Mourão" },
        new Linha { Nome = "Linha Ivaiporã - Cascavel", Chave = "ivp_cvo", ColunaA = "KMIVP", ColunaB = "KMCVO", ArquivoExcel = "KM IVP CVO.xlsx", Grupo = "Campo Mourão" },
        new Linha { Nome = "Linha Cascavel - Cascavel Oeste", Chave = "cvo_cvo", ColunaA = "CEL-CVO", ColunaB = "CVO-CEL", ArquivoExcel = "KM CEL CVO.xlsx", Grupo = "Campo Mourão" },
        new Linha { Nome = "Linha Cascavel - Guaira", Chave = "cvo_guaira", ColunaA = "CVO-GUI", ColunaB = "GUI-CVO", ArquivoExcel = "KM CVO GUI.xlsx", Grupo = "Campo Mourão" },
        new Linha { Nome = "Linha Areia - Ivaiporã", Chave = "are_ivp", ColunaA = "KMARE", ColunaB = "KMIVP", ArquivoExcel = "KM ARE IVP.xlsx", Grupo = "Campo Mourão" },
    };
     private static readonly Dictionary<string, string> _linhasGpx = new()
    {
        {"cmo_apa", "cmo_apucarana.gpx"},
        {"cmo_mga", "cmo_maringa.gpx"},
        {"cmo_sos", "cmo_salto_osorio.gpx"},
        {"cmo_sos c2", "cmo_salto_osorio_c2.gpx"},
        {"cmo_ssa", "cmo_salto_santiago.gpx"},
        {"cmo_ssac2", "cmo_salto_santiago_c2.gpx"},
        {"ivp_cvo", "ivp_cascavel.gpx"},
        {"are_ivp", "areia_ivaipora.gpx"},
        {"lonlns", "londrina_lns.gpx"},
        {"lonlna", "londrina_lna.gpx"},
        {"lonlna2", "londrina_lna2.gpx"},
        {"lon_sdi", "lon_sdi.gpx"},
        {"lon_mga", "lon_mga.gpx"},
        {"assis_c2_londrina_norte", "assis2.gpx"},
        {"lna_assis", "assisc1.gpx"},
        {"ivp_lon", "ivaipora_londrina.gpx"},
        {"apucarana", "apucarana.gpx"},
        {"cvo_cvo", "cvo_cvo.gpx"},
        {"cvo_guaira", "cvo_guaira.gpx"},
    };

    public LinhaService(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
        OfficeOpenXml.ExcelPackage.License.SetNonCommercialOrganization("onde-a-linha-desligou-web");
    }

    public IEnumerable<Linha> GetAll()
    {
        return _linhas;
    }

    public IEnumerable<Linha> GetByGroup(string grupo)
    {
        return _linhas.Where(l => l.Grupo.Equals(grupo, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<Torre> BuscarTorre(string chave, double? valorA, double? valorB)
    {
        var linha = _linhas.FirstOrDefault(l => l.Chave == chave);
        if (linha == null)
        {
            throw new Exception($"Linha com chave '{chave}' não encontrada.");
        }

        var resourcesPath = Path.Combine(_hostingEnvironment.ContentRootPath, "..", "..", "static", "resources");
        var excelPath = Path.Combine(resourcesPath, linha.ArquivoExcel);

        if (!File.Exists(excelPath))
        {
            throw new FileNotFoundException($"Arquivo Excel '{linha.ArquivoExcel}' não encontrado.");
        }

        using var package = new ExcelPackage(new FileInfo(excelPath));
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
        if (worksheet == null)
        {
            throw new Exception("Nenhuma planilha encontrada no arquivo Excel.");
        }

        var header = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column]
            .Select(c => c.Text.Trim().ToUpper())
            .ToList();

        var colAIndex = header.IndexOf(linha.ColunaA.ToUpper()) + 1;
        var colBIndex = header.IndexOf(linha.ColunaB.ToUpper()) + 1;
        var codigoIndex = header.IndexOf("CODIGO") + 1;
        var municipioIndex = header.IndexOf("MUNICIPIO") + 1;
        var setorIndex = header.IndexOf("SETOR") + 1;

        if (colAIndex == 0 || colBIndex == 0 || codigoIndex == 0 || municipioIndex == 0 || setorIndex == 0)
        {
            throw new Exception("Uma ou mais colunas necessárias não foram encontradas na planilha.");
        }

        double minDiff = double.MaxValue;
        int bestRow = -1;

        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            if (valorA.HasValue)
            {
                if (double.TryParse(worksheet.Cells[row, colAIndex].Text, out double valA))
                {
                    var diff = Math.Abs(valA - valorA.Value);
                    if (diff < minDiff)
                    {
                        minDiff = diff;
                        bestRow = row;
                    }
                }
            }

            if (valorB.HasValue)
            {
                if (double.TryParse(worksheet.Cells[row, colBIndex].Text, out double valB))
                {
                    var diff = Math.Abs(valB - valorB.Value);
                    if (diff < minDiff)
                    {
                        minDiff = diff;
                        bestRow = row;
                    }
                }
            }
        }

        if (bestRow == -1)
        {
            throw new Exception("Nenhuma torre correspondente encontrada na planilha.");
        }

        var codigoTorre = worksheet.Cells[bestRow, codigoIndex].Text;
        var cidade = worksheet.Cells[bestRow, municipioIndex].Text;
        var setor = worksheet.Cells[bestRow, setorIndex].Text;

        var codigoTorreAjustado = AjustarCodigoTorre(codigoTorre, chave);
        var numeroParaExibicao = ExtrairApenasNumero(codigoTorre) ?? "N/A";

        var gpxFile = _linhasGpx[chave];
        var gpxPath = Path.Combine(resourcesPath, gpxFile);

        var (latitude, longitude) = await BuscarTorreNoGpxAsync(codigoTorreAjustado, gpxPath);

        return new Torre
        {
            CodigoOriginal = codigoTorre,
            NumeroParaExibicao = numeroParaExibicao,
            Cidade = cidade,
            Setor = setor,
            Latitude = latitude,
            Longitude = longitude
        };
    }

    private async Task<(double, double)> BuscarTorreNoGpxAsync(string codigoTorre, string gpxPath)
    {
        if (!File.Exists(gpxPath))
        {
            throw new FileNotFoundException($"Arquivo GPX '{gpxPath}' não encontrado.");
        }

        var gpxContent = await File.ReadAllTextAsync(gpxPath);
        XDocument gpxDoc = XDocument.Parse(gpxContent);
        XNamespace gpxNs = "http://www.topografix.com/GPX/1/1";

        var waypoint = gpxDoc.Descendants(gpxNs + "wpt")
            .FirstOrDefault(wpt => wpt.Element(gpxNs + "name")?.Value.Trim() == codigoTorre);

        if (waypoint != null)
        {
            var latAttribute = waypoint.Attribute("lat");
            var lonAttribute = waypoint.Attribute("lon");

            if (latAttribute != null && lonAttribute != null && 
                double.TryParse(latAttribute.Value, out double lat) && 
                double.TryParse(lonAttribute.Value, out double lon))
            {
                return (lat, lon);
            }
            throw new Exception($"Coordenadas inválidas para a torre '{codigoTorre}' no arquivo GPX.");
        }

        throw new Exception($"Torre '{codigoTorre}' não encontrada no arquivo GPX.");
    }

    private string AjustarCodigoTorre(string codigoTorre, string dfKey)
    {
        if (string.IsNullOrWhiteSpace(codigoTorre)) return string.Empty;

        codigoTorre = codigoTorre.Trim();

        if (codigoTorre.Contains("TO"))
        {
            var parts = codigoTorre.Split(new[] { "TO" }, StringSplitOptions.None);
            if (parts.Length > 1)
            {
                var numeroStr = new string(parts[1].Where(char.IsDigit).ToArray());
                if (string.IsNullOrEmpty(numeroStr))
                {
                    throw new ValueError("Número da torre não encontrado após o prefixo 'TO'.");
                }

                if (dfKey == "lna_assis")
                {
                    return numeroStr.PadLeft(3, '0');
                }
                return int.Parse(numeroStr).ToString();
            }
        }

        var numero = new string(codigoTorre.Where(char.IsDigit).ToArray());
        if (string.IsNullOrEmpty(numero))
        {
            throw new ValueError("Número da torre não encontrado no código fornecido.");
        }

        return int.Parse(numero).ToString();
    }

    private string? ExtrairApenasNumero(string codigoTorre)
    {
        if (string.IsNullOrWhiteSpace(codigoTorre)) return null;

        codigoTorre = codigoTorre.Trim();

        if (codigoTorre.Contains("TO"))
        {
            var parts = codigoTorre.Split(new[] { "TO" }, StringSplitOptions.None);
            if (parts.Length > 1)
            {
                var numero = new string(parts[1].Where(char.IsDigit).ToArray());
                if (!string.IsNullOrEmpty(numero))
                {
                    return int.Parse(numero).ToString();
                }
            }
        }

        var match = Regex.Match(codigoTorre.ToUpper(), @"^[A-Z]*(\d+)([A-Z]*)$");
        if (match.Success)
        {
            var numero = match.Groups[1].Value;
            var sufixo = match.Groups[2].Value;
            return $"{int.Parse(numero)}{sufixo}";
        }

        var fallbackNumero = new string(codigoTorre.Where(char.IsDigit).ToArray());
        if (!string.IsNullOrEmpty(fallbackNumero))
        {
            return int.Parse(fallbackNumero).ToString();
        }

        return null;
    }
}

public class ValueError : Exception
{
    public ValueError(string message) : base(message) { }
}
