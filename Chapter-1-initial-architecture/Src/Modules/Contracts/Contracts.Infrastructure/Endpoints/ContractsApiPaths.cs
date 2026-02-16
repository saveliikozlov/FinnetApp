namespace EvolutionaryArchitecture.Contracts.Infrastructure.Endpoints;

public static class ContractsApiPaths
{
    private const string ContractsRootApi = "api/contracts";

    public const string Prepare = ContractsRootApi;
    public const string Sign = $"{ContractsRootApi}/{{id}}";
}
