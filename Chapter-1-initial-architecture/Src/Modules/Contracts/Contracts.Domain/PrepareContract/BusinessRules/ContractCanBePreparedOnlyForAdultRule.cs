namespace EvolutionaryArchitecture.Contracts.Domain.PrepareContract.BusinessRules;

using EvolutionaryArchitecture.Contracts.Domain.BusinessRules;

internal sealed class ContractCanBePreparedOnlyForAdultRule : IBusinessRule
{
    private readonly int _age;

    internal ContractCanBePreparedOnlyForAdultRule(int age) => _age = age;

    public bool IsMet() => _age >= 18;

    public string ErrorMessage => "Contract can not be prepared for a person who is not adult";
}
