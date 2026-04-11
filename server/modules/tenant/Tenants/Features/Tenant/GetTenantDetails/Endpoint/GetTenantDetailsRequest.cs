using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Tenants.Features.Tenant.GetTenantDetails.Endpoint;

public record GetTenantDetailsRequest(string CognitoId); 

public class GetTenantDetailsRequestValidator : Validator<GetTenantDetailsRequest>
{
    public GetTenantDetailsRequestValidator()
    {
        RuleFor(x => x.CognitoId)
            .NotEmpty()
            .WithMessage("CognitoId is required")
            .MustBeUuid();
    }
}