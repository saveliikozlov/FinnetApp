namespace EvolutionaryArchitecture.Fitnet.UnitTests.Contracts.PrepareContract.BusinessRules;

using EvolutionaryArchitecture.Contracts.Domain.PrepareContract.BusinessRules;
using EvolutionaryArchitecture.Contracts.Domain.BusinessRules;

public sealed class PreviousContractHasToBeSignedRuleTests
{
    [Fact]
    internal void Given_previous_contract_signed_Then_validation_should_pass() =>
        // Arrange
        // Act & Assert
        Should.NotThrow(() => BusinessRuleValidator.Validate(new PreviousContractHasToBeSignedRule(true)));

    [Fact]
    internal void Given_previous_contract_not_exists_Then_validation_should_pass() =>
        // Arrange
        // Act & Assert
        Should.NotThrow(() => BusinessRuleValidator.Validate(new PreviousContractHasToBeSignedRule(null)));

    [Fact]
    internal void Given_previous_contract_unsigned_Then_validation_should_throw()
    {
        // Arrange

        // Act
        var exception = Should.Throw<BusinessRuleValidationException>(() => BusinessRuleValidator.Validate(new PreviousContractHasToBeSignedRule(false)));

        // Assert
        exception.Message.ShouldBe("Previous contract must be signed by the customer");
    }
}
