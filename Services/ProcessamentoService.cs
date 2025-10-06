using System.Diagnostics;

namespace CancelationTokenExemplo.Services
{
    public class ProcessamentoService : IProcessamentoService
    {
        // =========================================================================
        // IMPLEMENTAÇÃO 1: COOPERATIVO
        // =========================================================================
        public async Task<IResult> ProcessarCooperativoAsync(ILogger logger, CancellationToken ct)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();
                string nomeOperacao = "COOPERATIVO";

                logger.LogInformation("Endpoint {Nome}: Iniciado.", nomeOperacao);

                await SimularTrabalhoPesadoAsync(ct, nomeOperacao, logger, true); // Passando 'true' para cooperativo

                stopwatch.Stop();

                logger.LogInformation("[{Nome}] CONCLUÍDO. Duração total: {Duracao}ms", nomeOperacao, stopwatch.ElapsedMilliseconds);

                return Results.Ok($"{nomeOperacao}: Relatório gerado com sucesso.");
            }
            catch (OperationCanceledException)
            {
                // Interrompe e retorna o código 499 (Requisição Cancelada pelo Cliente)
                logger.LogWarning("Endpoint COOPERATIVO: CANCELADO (Requisição Fechada).");
                return Results.Problem(
                    detail: "A operação foi interrompida devido ao cancelamento do cliente.",
                    statusCode: 499,
                    title: "Requisição Cancelada");
            }
        }

        // =========================================================================
        // IMPLEMENTAÇÃO 2: NÃO COOPERATIVO
        // =========================================================================
        public async Task<IResult> ProcessarNaoCooperativoAsync(ILogger logger)
        {
            var stopwatch = Stopwatch.StartNew();
            string nomeOperacao = "NAO-COOPERATIVO";

            logger.LogInformation("Endpoint {Nome}: Iniciado.", nomeOperacao);
            
            // Simulação que ignora o token.
            await SimularTrabalhoPesadoAsync(CancellationToken.None, nomeOperacao, logger, false); 

            stopwatch.Stop();

            logger.LogInformation("[{Nome}] CONCLUÍDO. Duração total: {Duracao}ms", nomeOperacao, stopwatch.ElapsedMilliseconds);

            return Results.Ok($"{nomeOperacao}: Relatório gerado. (Pode ter demorado mais do que o cliente esperava).");
        }

        // Lógica de simulação (agora um método privado da classe)
        private static async Task SimularTrabalhoPesadoAsync(
            CancellationToken cancellationToken, 
            string nomeOperacao, 
            ILogger logger,
            bool isCooperative)
        {
            for (int i = 1; i <= 5; i++)
            {
                // Task.Delay verifica o token (cooperativo) ou ignora (não cooperativo)
                await Task.Delay(1000, cancellationToken); 
                
                // Ponto extra: forçar a verificação no lado cooperativo, caso Task.Delay não fosse usado
                if (isCooperative)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }

                logger.LogInformation("[{nomeOperacao}] Etapa {i} de 5 concluída.", nomeOperacao, i);
            }
        }
    }
}