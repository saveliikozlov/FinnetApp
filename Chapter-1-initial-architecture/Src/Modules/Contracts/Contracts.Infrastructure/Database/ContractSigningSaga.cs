namespace EvolutionaryArchitecture.Contracts.Infrastructure.Database;

public enum SagaStatus { Started, Completed, Failed }

public class ContractSigningSaga
{
    public Guid SagaId { get; }
    public Guid CorrelationId { get; }  // ContractId
    public SagaStatus Status { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; private set; }

    public ContractSigningSaga(Guid sagaId, Guid correlationId, DateTime createdAt)
    {
        SagaId = sagaId;
        CorrelationId = correlationId;
        Status = SagaStatus.Started;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public void Complete(DateTime updatedAt)
    {
        Status = SagaStatus.Completed;
        UpdatedAt = updatedAt;
    }

    public void Fail(DateTime updatedAt)
    {
        Status = SagaStatus.Failed;
        UpdatedAt = updatedAt;
    }

    private ContractSigningSaga() { }
}
