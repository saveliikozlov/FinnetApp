namespace EvolutionaryArchitecture.Contracts.Domain.PrepareContract.BusinessRules;

using EvolutionaryArchitecture.Contracts.Domain.BusinessRules;

internal sealed class PreviousContractHasToBeSignedRule : IBusinessRule
{
    private readonly bool? _signed;

    internal PreviousContractHasToBeSignedRule(bool? signed) => _signed = signed;
    public bool IsMet() => _signed is true or null;
    public string ErrorMessage => "Previous contract must be signed by the customer";
}
