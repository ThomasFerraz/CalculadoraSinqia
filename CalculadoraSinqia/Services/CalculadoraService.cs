using Microsoft.EntityFrameworkCore;

public class CalculadoraService : ICalculadoraService
{
    private readonly AppDbContext _context;
    private readonly ILogger<CalculadoraService> _logger;

    public CalculadoraService(AppDbContext context, ILogger<CalculadoraService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<InvestimentoResponse> CalcularAsync(InvestimentoRequest request)
    {
        if (request.DataFinal <= request.DataAplicacao)
            throw new InvalidOperationException("Data final deve ser maior que data de aplicação.");

        // Pular o primeiro dia: começamos no dia seguinte à aplicação
        var dataAtual = request.DataAplicacao.AddDays(1);

        decimal fatorAcumulado = 1m;

        while (dataAtual <= request.DataFinal)
        {
            // Ignorar finais de semana
            if (dataAtual.DayOfWeek != DayOfWeek.Saturday && dataAtual.DayOfWeek != DayOfWeek.Sunday)
            {
                // Buscar a cotação do dia anterior útil
                var dataCotacao = await BuscarUltimoDiaUtilAnterior(dataAtual);
                var cotacaoAnterior = _context.Cotacao
                    .AsEnumerable()
                    .FirstOrDefault(c => c.Indexador == "SQI" &&
                                         c.Data.ToString("yyyy-MM-dd") == dataCotacao.ToString("yyyy-MM-dd"));

                if (cotacaoAnterior == null)
                    throw new InvalidOperationException($"Cotação não encontrada para {dataCotacao:yyyy-MM-dd}");

                // Calcular fator diário
                var taxaAnual = cotacaoAnterior.Valor;
                var fatorDiario = (decimal)Math.Pow((double)(1 + taxaAnual / 100), 1.0 / 252);

                // Truncar fator diário para 8 casas decimais
                fatorDiario = TruncarDecimal(fatorDiario, 8);

                // Multiplicar no fator acumulado
                fatorAcumulado *= fatorDiario;
            }

            dataAtual = dataAtual.AddDays(1); // Próximo dia
        }

        // Truncar fator acumulado na 16ª casa decimal
        fatorAcumulado = TruncarDecimal(fatorAcumulado, 16);

        // Calcular valor atualizado
        var valorAtualizado = TruncarDecimal(request.ValorInicial * fatorAcumulado, 8);

        return new InvestimentoResponse
        {
            FatorJuros = fatorAcumulado,
            ValorFinal = valorAtualizado
        };
    }

    private async Task<DateTime> BuscarUltimoDiaUtilAnterior(DateTime data)
    {
        var anterior = data.AddDays(-1);

        // Se anterior for sábado ou domingo, volta mais
        while (anterior.DayOfWeek == DayOfWeek.Saturday || anterior.DayOfWeek == DayOfWeek.Sunday)
        {
            anterior = anterior.AddDays(-1);
        }

        return anterior;
    }

    // Método para truncar decimais
    private decimal TruncarDecimal(decimal valor, int casas)
    {
        var fator = (decimal)Math.Pow(10, casas);
        return Math.Truncate(valor * fator) / fator;
    }
}