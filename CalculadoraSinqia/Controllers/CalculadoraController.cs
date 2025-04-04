using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CalculadoraController : ControllerBase
{
    private readonly ICalculadoraService _calculadoraService;
    private readonly ILogger<CalculadoraController> _logger;

    public CalculadoraController(ICalculadoraService calculadoraService, ILogger<CalculadoraController> logger)
    {
        _calculadoraService = calculadoraService;
        _logger = logger;
    }

    [HttpPost("calcular")]
    public async Task<IActionResult> Calcular([FromBody] InvestimentoRequest request)
    {
        if (request.DataFinal <= request.DataAplicacao)
            return BadRequest("Data final deve ser maior que a data de aplicação.");

        try
        {
            var resultado = await _calculadoraService.CalcularAsync(request);
            return Ok(resultado);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro ao calcular investimento");
            return NotFound(ex.Message);
        }
    }
}