
using Microsoft.AspNetCore.Mvc;
using OndeALinhaDesligouWeb.Models;
using OndeALinhaDesligouWeb.Services;

namespace OndeALinhaDesligouWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LinhasController : ControllerBase
{
    private readonly LinhaService _linhaService;

    public LinhasController(LinhaService linhaService)
    {
        _linhaService = linhaService;
    }

    [HttpGet]
    public IEnumerable<Linha> GetAll()
    {
        return _linhaService.GetAll();
    }

    [HttpGet("{grupo}")]
    public ActionResult<IEnumerable<Linha>> GetByGroup(string grupo)
    {
        var linhas = _linhaService.GetByGroup(grupo);
        if (!linhas.Any())
        {
            return NotFound();
        }
        return Ok(linhas);
    }

    [HttpGet("buscar")]
    public async Task<ActionResult<Torre>> BuscarTorre([FromQuery] string chave, [FromQuery] double? valorA, [FromQuery] double? valorB)
    {
        if (string.IsNullOrWhiteSpace(chave) || (!valorA.HasValue && !valorB.HasValue))
        {
            return BadRequest("A 'chave' da linha e pelo menos um valor de KM (valorA ou valorB) são obrigatórios.");
        }

        try
        {
            var torre = await _linhaService.BuscarTorre(chave, valorA, valorB);
            return Ok(torre);
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
