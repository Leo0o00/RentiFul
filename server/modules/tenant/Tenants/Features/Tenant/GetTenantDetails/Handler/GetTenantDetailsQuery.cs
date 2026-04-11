using Ardalis.Result;
using FluentValidation;
using Mediator;
using Tenants.Contracts;

namespace Tenants.Features.Tenant.CreateTenant.Handler;

public record GetTenantDetailsQuery(
    string CognitoId
) : IRequest<Result<TenantDetailsDto>>;

public class GetTenantDetailsQueryValidator : AbstractValidator<GetTenantDetailsQuery>
{
    public GetTenantDetailsQueryValidator()
    {
        RuleFor(x => x.CognitoId)
            .NotEmpty()
            .WithMessage("CognitoId is required")
            .MustBeUuid();
    }
}