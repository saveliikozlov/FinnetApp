namespace EvolutionaryArchitecture.Contracts.Domain.BusinessRules;

public interface IBusinessRule
{
    bool IsMet();
    string ErrorMessage { get; }
}
