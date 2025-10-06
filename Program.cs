using CancelationTokenExemplo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProcessamentoService, ProcessamentoService>();
builder.Services.AddLogging();

var app = builder.Build();

// =========================================================================
// ENDPOINT 1: COOPERATIVO (USA CancellationToken)
// =========================================================================
app.MapGet("/with-cancelation-token", async (IProcessamentoService service, ILogger<Program> logger, CancellationToken ct) =>
{
    // PASSO 2: A lógica inteira está na classe de serviço
    await service.ProcessarCooperativoAsync(logger, ct);
});

// =========================================================================
// ENDPOINT 2: NÃO COOPERATIVO (IGNORA CancellationToken)
// =========================================================================
app.MapGet("/without-cancelation-token", async (IProcessamentoService service, ILogger<Program> logger) =>
{
    // O ILogger<Program> é injetado para que o serviço possa logar
    return await service.ProcessarNaoCooperativoAsync(logger);
});

app.Run();