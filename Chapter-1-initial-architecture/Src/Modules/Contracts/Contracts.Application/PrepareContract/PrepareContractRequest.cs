namespace EvolutionaryArchitecture.Contracts.Application.PrepareContract;

public sealed record PrepareContractRequest(Guid CustomerId, int CustomerAge, int CustomerHeight, DateTimeOffset PreparedAt);
