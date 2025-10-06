namespace CancelationTokenExemplo.Services
{
    public interface IProcessamentoService
    {
        // Método que honra o cancelamento
        Task<IResult> ProcessarCooperativoAsync(ILogger logger, CancellationToken ct);

        // Método que ignora o cancelamento
        Task<IResult> ProcessarNaoCooperativoAsync(ILogger logger);
    }
}