using Ardalis.Result;
using FluentValidation;
using Mediator;
using Tenants.Contracts;

namespace Tenants.Features.Property.GetTenantOwnedProperties.Handler;

public record GetTenantOwnedPropertiesQuery(
    string CognitoId
) : IRequest<Result<TenantOwnedPropertiesDto>>;

public class GetTenantOwnedPropertiesQueryValidator : AbstractValidator<GetTenantOwnedPropertiesQuery>
{
    public GetTenantOwnedPropertiesQueryValidator()
    {
        RuleFor(x => x.CognitoId)
            .NotEmpty()
            .WithMessage("CognitoId is required")
            .MaximumLength(256)
            .WithMessage("CognitoId cannot exceed 256 characters length");
    }
}