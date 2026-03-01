namespace EvolutionaryArchitecture.Contracts.Infrastructure.Database;

using EvolutionaryArchitecture.Contracts.Domain;
using Microsoft.EntityFrameworkCore;

internal sealed class ContractsPersistence(DbContextOptions<ContractsPersistence> options) : DbContext(options)
{
    private const string Schema = "Contracts";

    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<ContractSigningSaga> ContractSigningSagas => Set<ContractSigningSaga>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfiguration(new ContractEntityConfiguration());
        modelBuilder.Entity<OutboxMessage>(b =>
        {
            b.ToTable("OutboxMessages");
            b.HasKey(x => x.Id);
        });
        modelBuilder.Entity<ContractSigningSaga>(b =>
        {
            b.ToTable("ContractSigningSagas");
            b.HasKey(x => x.SagaId);
            b.HasIndex(x => x.CorrelationId).IsUnique();
            b.Property(x => x.Status).HasConversion<string>();
        });
    }
}
