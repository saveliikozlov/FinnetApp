namespace EvolutionaryArchitecture.Contracts.Domain.BusinessRules;

public class BusinessRuleValidationException : InvalidOperationException
{
    internal BusinessRuleValidationException(string message) : base(message)
    {
    }
}
