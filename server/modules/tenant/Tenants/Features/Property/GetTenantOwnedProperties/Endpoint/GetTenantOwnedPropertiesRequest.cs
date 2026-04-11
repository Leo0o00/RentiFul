using FastEndpoints;
using FluentValidation;

namespace Tenants.Features.Property.GetTenantOwnedProperties.Endpoint;

public record GetTenantOwnedPropertiesRequest(
    string CognitoId
);
    


public class GetTenantOwnedPropertiesRequestValidator : Validator<GetTenantOwnedPropertiesRequest>
{
    public GetTenantOwnedPropertiesRequestValidator()
    {
        RuleFor(x => x.CognitoId)
            .NotEmpty()
            .WithMessage("CognitoId is required")
            .MaximumLength(256)
            .WithMessage("CognitoId cannot exceed 256 characters length");
    }
}