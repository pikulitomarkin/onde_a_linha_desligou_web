
using OndeALinhaDesligouWeb.Models;
using OfficeOpenXml;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace OndeALinhaDesligouWeb.Services;

using Microsoft.Extensions.Options;
using OndeALinhaDesligouWeb.Options;

public class LinhaService
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly List<Linha> _linhas;
    private readonly Dictionary<string, string> _linhasGpx;
    private readonly string _resourcesPath;

    public LinhaService(IWebHostEnvironment hostingEnvironment, LinhasOptions options)
    {
        _hostingEnvironment = hostingEnvironment;
        _linhas = options.Linhas ?? new List<Linha>();
        _linhasGpx = options.LinhasGpx ?? new Dictionary<string, string>();
        _resourcesPath = string.IsNullOrWhiteSpace(options.ResourcesPath) ? "static/resources" : options.ResourcesPath;
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

    var resourcesPath = Path.Combine(_hostingEnvironment.ContentRootPath, "..", "..", _resourcesPath);
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
