namespace EvolutionaryArchitecture.Contracts.Application;

using PrepareContract;

public interface IContractsService
{
    Task<Guid> PrepareContractAsync(PrepareContractRequest request, CancellationToken cancellationToken = default);
    Task SignContractAsync(Guid contractId, DateTimeOffset signedAt, CancellationToken cancellationToken = default);
}
