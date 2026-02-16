namespace EvolutionaryArchitecture.Fitnet.Common.Events.Adapters;

using MediatR;
using ContractsEventBus = EvolutionaryArchitecture.Contracts.Application.IEventBus;

internal sealed class ContractsEventBusAdapter(IMediator mediator) : ContractsEventBus
{
    public async Task PublishAsync<TEvent>(TEvent notification, CancellationToken cancellationToken = default)
        where TEvent : INotification =>
        await mediator.Publish(notification, cancellationToken);
}
