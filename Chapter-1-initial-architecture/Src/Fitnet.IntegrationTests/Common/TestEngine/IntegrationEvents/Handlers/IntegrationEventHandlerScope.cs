namespace EvolutionaryArchitecture.Fitnet.IntegrationTests.Common.TestEngine.IntegrationEvents.Handlers;

using MediatR;

internal sealed class IntegrationEventHandlerScope<TIntegrationEvent> : IDisposable
where TIntegrationEvent : INotification
{
    private readonly IServiceScope _serviceScope;
    internal readonly INotificationHandler<TIntegrationEvent> IntegrationEventHandler;

    public IntegrationEventHandlerScope(WebApplicationFactory<Program> applicationInMemoryFactory)
    {
        _serviceScope = applicationInMemoryFactory.Services.CreateScope();
        IntegrationEventHandler = _serviceScope
            .ServiceProvider
            .GetRequiredService<INotificationHandler<TIntegrationEvent>>();
    }

    public async Task Consume(TIntegrationEvent @event, CancellationToken cancellationToken = default) =>
        await IntegrationEventHandler.Handle(@event, cancellationToken);

    public void Dispose() =>
        _serviceScope.Dispose();
}
