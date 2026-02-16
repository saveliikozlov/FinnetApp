namespace EvolutionaryArchitecture.Contracts.Infrastructure.Endpoints;

using Application;
using Application.PrepareContract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

internal static class PrepareContractEndpoint
{
    internal static void MapPrepareContract(IEndpointRouteBuilder app) =>
        app.MapPost(ContractsApiPaths.Prepare,
                async (PrepareContractRequest request,
                    IContractsService contractsService,
                    CancellationToken cancellationToken) =>
                {
                    var contractId = await contractsService.PrepareContractAsync(request, cancellationToken);

                    return Results.Created($"/{ContractsApiPaths.Prepare}/{contractId}", contractId);
                })
            .WithSummary("Triggers preparation of a new contract for new or existing customer")
            .WithDescription("This endpoint is used to prepare a new contract for new and existing customers.")
            .Produces<string>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status500InternalServerError);
}
