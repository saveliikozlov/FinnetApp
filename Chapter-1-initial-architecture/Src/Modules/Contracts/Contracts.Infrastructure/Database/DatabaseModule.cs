namespace EvolutionaryArchitecture.Contracts.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

internal static class DatabaseModule
{
    internal static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ContractsPersistenceOptions>(
            configuration.GetSection(ContractsPersistenceOptions.SectionName));
        services.AddOptionsWithValidateOnStart<ContractsPersistenceOptions>();
        services.AddDbContext<ContractsPersistence>((serviceProvider, options) =>
        {
            var persistenceOptions = serviceProvider.GetRequiredService<IOptions<ContractsPersistenceOptions>>();
            var connectionString = persistenceOptions.Value.Contracts;
            options.UseNpgsql(connectionString);
        });

        return services;
    }

    internal static void UseDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ContractsPersistence>();
        context.Database.Migrate();
    }
}
