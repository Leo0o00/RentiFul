using Ardalis.Result;
using FluentValidation;
using Mediator;
using Tenants.Contracts;

namespace Tenants.Features.Property.AddFavoriteProperty.Handler;

public record AddFavoritePropertyCommand(
    string CognitoId,
    string PropertyId
) : IRequest<Result<Guid>>;

public class AddFavoritePropertyCommandValidator : AbstractValidator<AddFavoritePropertyCommand>
{
    public AddFavoritePropertyCommandValidator()
    {
        RuleFor(x => x.CognitoId)
            .NotEmpty()
            .WithMessage("CognitoId is required")
            .MaximumLength(256)
            .WithMessage("CognitoId cannot exceed 256 characters length");
        RuleFor(x => x.PropertyId)
            .NotEmpty()
            .WithMessage("PropertyId is required")
            .Must(BeAValidGuid)
            .WithMessage("PropertyId must be a valid GUID");
    }

    private bool BeAValidGuid(string guid)
    {
        return Guid.TryParse(guid, out _);
    }
}