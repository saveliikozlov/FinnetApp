namespace EvolutionaryArchitecture.Contracts.Application;

using MediatR;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent notification, CancellationToken cancellationToken = default)
        where TEvent : INotification;
}
