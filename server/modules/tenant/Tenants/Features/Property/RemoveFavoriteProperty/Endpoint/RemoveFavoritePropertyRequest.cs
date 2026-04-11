using FastEndpoints;
using FluentValidation;

namespace Tenants.Features.Property.RemoveFavoriteProperty.Endpoint;

public record RemoveFavoritePropertyRequest(
    string CognitoId,
    string PropertyId
);
    


public class RemoveFavoritePropertyRequestValidator : Validator<RemoveFavoritePropertyRequest>
{
    public RemoveFavoritePropertyRequestValidator()
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
            .WithMessage("PropertyId must be a valid GUID");;
    }
    
    private bool BeAValidGuid(string guid)
    {
        return Guid.TryParse(guid, out _);
    }
}