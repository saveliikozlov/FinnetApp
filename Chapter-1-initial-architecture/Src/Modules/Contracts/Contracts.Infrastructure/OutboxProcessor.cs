namespace EvolutionaryArchitecture.Contracts.Infrastructure;

using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

internal sealed partial class OutboxProcessor(
    IServiceScopeFactory scopeFactory,
    ILogger<OutboxProcessor> logger,
    TimeProvider timeProvider) : BackgroundService
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Processing outbox message {Id} of type {Type}: {Payload}")]
    private partial void LogProcessingMessage(Guid id, string type, string payload);

    [LoggerMessage(Level = LogLevel.Information, Message = "Saga {SagaId} for contract {ContractId} marked as Completed")]
    private partial void LogSagaCompleted(Guid sagaId, Guid contractId);

    [LoggerMessage(Level = LogLevel.Information, Message = "Saga {SagaId} already Completed, skipping")]
    private partial void LogSagaAlreadyCompleted(Guid sagaId);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessMessagesAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }

    private async Task ProcessMessagesAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var persistence = scope.ServiceProvider.GetRequiredService<ContractsPersistence>();

        var messages = await persistence.OutboxMessages
            .Where(m => m.ProcessedAt == null)
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            await HandleMessageAsync(persistence, message, cancellationToken);
        }
    }

    private async Task HandleMessageAsync(ContractsPersistence persistence, OutboxMessage message, CancellationToken cancellationToken)
    {
        LogProcessingMessage(message.Id, message.Type, message.Payload);

        // Parse CorrelationId from payload and advance Saga state
        var payload = System.Text.Json.JsonDocument.Parse(message.Payload);
        if (payload.RootElement.TryGetProperty("id", out var idElement) &&
            Guid.TryParse(idElement.GetString(), out var contractId))
        {
            var saga = await persistence.ContractSigningSagas
                .SingleOrDefaultAsync(s => s.CorrelationId == contractId, cancellationToken);

            if (saga is not null && saga.Status == SagaStatus.Started)
            {
                saga.Complete(timeProvider.GetUtcNow().UtcDateTime);
                LogSagaCompleted(saga.SagaId, contractId);
            }
            else if (saga?.Status == SagaStatus.Completed)
            {
                LogSagaAlreadyCompleted(saga.SagaId);
            }
        }

        message.ProcessedAt = timeProvider.GetUtcNow().UtcDateTime;
        await persistence.SaveChangesAsync(cancellationToken);
    }
}
