namespace EvolutionaryArchitecture.Fitnet.Passes.RegisterPass;

using EvolutionaryArchitecture.Contracts.Application.SignContract.Events;
using Data;
using Data.Database;
using Events;
using EvolutionaryArchitecture.Fitnet.Common.Events.EventBus;
using MediatR;

internal sealed class ContractSignedEventHandler(
    PassesPersistence persistence,
    IEventBus eventBus) : INotificationHandler<ContractSignedEvent>
{
    public async Task Handle(ContractSignedEvent @event, CancellationToken cancellationToken)
    {
        var pass = Pass.Register(@event.ContractCustomerId, @event.SignedAt, @event.ExpireAt);
        await persistence.Passes.AddAsync(pass, cancellationToken);
        await persistence.SaveChangesAsync(cancellationToken);

        var passRegisteredEvent = PassRegisteredEvent.Create(pass.Id);
        await eventBus.PublishAsync(passRegisteredEvent, cancellationToken);
    }
}
