using FastEndpoints;
using FluentValidation;

namespace Tenants.Features.Property.AddFavoriteProperty.Endpoint;

public record AddFavoritePropertyRequest(
    string CognitoId,
    string PropertyId
);
    


public class AddFavoritePropertyRequestValidator : Validator<AddFavoritePropertyRequest>
{
    public AddFavoritePropertyRequestValidator()
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