namespace EvolutionaryArchitecture.Contracts.Infrastructure;

using Application;
using Application.PrepareContract;
using Application.SignContract.Events;
using Database;
using EvolutionaryArchitecture.Contracts.Domain;
using Microsoft.EntityFrameworkCore;

internal sealed class ContractsService(
    ContractsPersistence persistence,
    IEventBus bus,
    TimeProvider timeProvider) : IContractsService
{
    public async Task<Guid> PrepareContractAsync(PrepareContractRequest request, CancellationToken cancellationToken = default)
    {
        var previousContract = await persistence.Contracts
            .OrderByDescending(contract => contract.PreparedAt)
            .SingleOrDefaultAsync(contract => contract.CustomerId == request.CustomerId, cancellationToken);

        var contract = Contract.Prepare(
            request.CustomerId,
            request.CustomerAge,
            request.CustomerHeight,
            request.PreparedAt,
            previousContract?.IsSigned);

        await persistence.Contracts.AddAsync(contract, cancellationToken);
        await persistence.SaveChangesAsync(cancellationToken);

        return contract.Id;
    }

    public async Task SignContractAsync(Guid contractId, DateTimeOffset signedAt, CancellationToken cancellationToken = default)
    {
        var contract = await persistence.Contracts.FindAsync([contractId], cancellationToken: cancellationToken) ??
            throw new InvalidOperationException($"Contract with id {contractId} was not found");

        var dateNow = timeProvider.GetUtcNow();
        contract.Sign(signedAt, dateNow);

        var now = dateNow.UtcDateTime;

        var outboxMessage = new OutboxMessage(
            Guid.NewGuid(),
            nameof(ContractSignedEvent),
            System.Text.Json.JsonSerializer.Serialize(new { @event = "ContractSigned", id = contract.Id }),
            now);
        await persistence.OutboxMessages.AddAsync(outboxMessage, cancellationToken);

        var existingSaga = await persistence.ContractSigningSagas
            .SingleOrDefaultAsync(s => s.CorrelationId == contract.Id, cancellationToken);
        if (existingSaga is null)
        {
            var saga = new ContractSigningSaga(Guid.NewGuid(), contract.Id, now);
            await persistence.ContractSigningSagas.AddAsync(saga, cancellationToken);
        }

        await persistence.SaveChangesAsync(cancellationToken);

        var @event = ContractSignedEvent.Create(
            contract.Id,
            contract.CustomerId,
            contract.SignedAt!.Value,
            contract.ExpiringAt!.Value,
            timeProvider.GetUtcNow());
        await bus.PublishAsync(@event, cancellationToken);
    }
}
