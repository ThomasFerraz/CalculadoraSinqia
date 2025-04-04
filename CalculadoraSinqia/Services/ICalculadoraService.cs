public interface ICalculadoraService
{
    Task<InvestimentoResponse> CalcularAsync(InvestimentoRequest request);
}