namespace EvolutionaryArchitecture.Contracts.Infrastructure;

using Database;
using EvolutionaryArchitecture.Contracts.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ContractsModule
{
    public static IServiceCollection AddContracts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddScoped<IContractsService, ContractsService>();
        services.AddHostedService<OutboxProcessor>();

        return services;
    }

    public static IApplicationBuilder UseContracts(this IApplicationBuilder applicationBuilder)
    {
        DatabaseModule.UseDatabase(applicationBuilder.ApplicationServices);

        return applicationBuilder;
    }

    public static void MapContracts(this IEndpointRouteBuilder app)
    {
        Endpoints.PrepareContractEndpoint.MapPrepareContract(app);
        Endpoints.SignContractEndpoint.MapSignContract(app);
    }
}
