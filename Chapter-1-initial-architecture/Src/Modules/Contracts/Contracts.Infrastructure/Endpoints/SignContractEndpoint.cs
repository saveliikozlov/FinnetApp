namespace EvolutionaryArchitecture.Contracts.Infrastructure.Endpoints;

using Application;
using Application.SignContract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

internal static class SignContractEndpoint
{
    internal static void MapSignContract(IEndpointRouteBuilder app) => app.MapPatch(ContractsApiPaths.Sign,
            async (Guid id, SignContractRequest request,
                IContractsService contractsService,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    await contractsService.SignContractAsync(id, request.SignedAt, cancellationToken);
                    return Results.NoContent();
                }
                catch (InvalidOperationException ex) when (ex.Message.Contains("was not found"))
                {
                    return Results.NotFound();
                }
            })
        .WithSummary("Signs prepared contract")
        .WithDescription("This endpoint is used to sign prepared contract by customer.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status500InternalServerError);
}
