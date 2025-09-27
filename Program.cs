var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// 2. Definição do Endpoint (A Magia do Minimal API)
// O CancellationToken 'ct' é injetado automaticamente pelo framework.
app.MapGet("/relatorio-lento", async (CancellationToken ct) =>
{
    try
    {
        // Chama a função que contém a lógica de longa duração, passando o token.
        bool operacaoConcluida = await SimularOperacaoPesadaAsync(ct);

        if (operacaoConcluida)
        {
            return Results.Ok("Relatório gerado com sucesso.");
        }
        else
        {
            // Este caminho é atingido se o cancelamento for solicitado,
            // mas a exceção OperationCanceledException não for lançada (veja a lógica de simulação).
            return Results.StatusCode(400); // Bad Request ou outro status apropriado
        }
    }
    // 3. Tratamento da Exceção de Cancelamento
    // Operações assíncronas que recebem o token e são canceladas lançam esta exceção.
    catch (OperationCanceledException)
    {
        // O código 499 (Client Closed Request) é uma boa prática
        // para indicar que o cliente interrompeu a espera.
        app.Logger.LogWarning("Requisição /relatorio-lento foi cancelada pelo cliente.");
        return Results.StatusCode(499);
    }
});

app.Run();

// 4. A Lógica de Longa Duração (Simulação)

/// <summary>
/// Simula uma tarefa que verifica periodicamente o sinal de cancelamento.
/// </summary>
async Task<bool> SimularOperacaoPesadaAsync(CancellationToken cancellationToken)
{
    // Simula 5 etapas de processamento, cada uma levando 1 segundo.
    for (int i = 1; i <= 5; i++)
    {
        // 4.1. Verificação Cooperativa (Ação Idiomática)
        // O Task.Delay aceita o token e irá lançar a exceção se o sinal for TRUE.
        await Task.Delay(1000, cancellationToken); 
        
        // Se a linha acima não lançar, podemos fazer uma verificação manual
        // para interromper loops que não contenham awaits:
        if (cancellationToken.IsCancellationRequested)
        {
            // Opcional: Limpar recursos aqui.
            return false; // Sai do loop e retorna false.
        }

        Console.WriteLine($"Etapa {i} concluída. (Thread: {Thread.CurrentThread.ManagedThreadId})");
    }

    return true; // Operação concluída.
}