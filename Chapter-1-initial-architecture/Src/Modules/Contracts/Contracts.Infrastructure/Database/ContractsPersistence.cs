namespace EvolutionaryArchitecture.Contracts.Infrastructure.Database;

using EvolutionaryArchitecture.Contracts.Domain;
using Microsoft.EntityFrameworkCore;

internal sealed class ContractsPersistence(DbContextOptions<ContractsPersistence> options) : DbContext(options)
{
    private const string Schema = "Contracts";

    public DbSet<Contract> Contracts => Set<Contract>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfiguration(new ContractEntityConfiguration());
    }
}
