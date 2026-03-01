namespace EvolutionaryArchitecture.Contracts.Infrastructure.Database;

public class OutboxMessage
{
    public Guid Id { get; }
    public string Type { get; } = string.Empty;
    public string Payload { get; } = string.Empty;
    public DateTime CreatedAt { get; }
    public DateTime? ProcessedAt { get; set; }

    public OutboxMessage(Guid id, string type, string payload, DateTime createdAt)
    {
        Id = id;
        Type = type;
        Payload = payload;
        CreatedAt = createdAt;
    }

    private OutboxMessage() { }
}
