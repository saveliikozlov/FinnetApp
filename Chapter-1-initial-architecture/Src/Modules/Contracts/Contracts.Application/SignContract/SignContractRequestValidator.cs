namespace EvolutionaryArchitecture.Contracts.Application.SignContract;

using FluentValidation;

internal sealed class SignContractRequestValidator : AbstractValidator<SignContractRequest>
{
    public SignContractRequestValidator() => RuleFor(signContractRequest => signContractRequest.SignedAt)
            .NotEmpty();
}
