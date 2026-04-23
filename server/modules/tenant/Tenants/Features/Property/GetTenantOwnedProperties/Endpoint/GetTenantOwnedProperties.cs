using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Tenants.Contracts;
using Tenants.Features.Tenant.CreateTenant.Endpoint;
using Tenants.Features.Tenant.CreateTenant.Handler;

namespace Tenants.Features.Property.GetTenantOwnedProperties.Endpoint;



public class GetTenantOwnedProperties : Endpoint<GetTenantOwnedPropertiesRequest, Result<TenantOwnedPropertiesDto>>
{
    private readonly IMediator _mediator;

    public GetTenantOwnedProperties(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get($"/{Constants.ModuleName}/" + "{CognitoId}/current-residences");
        Roles("tenant");
        Options(x => x.RequireRateLimiting("per-user"));
    }

    public override async Task HandleAsync(GetTenantOwnedPropertiesRequest request, CancellationToken cancellationToken = default)
    {
        var query = new Handler.GetTenantOwnedPropertiesQuery(
            request.CognitoId
            );
        
        var result = await _mediator.Send(query, cancellationToken);

        if (result.Status == ResultStatus.NotFound)
        {
            await HttpContext.Response.SendResultAsync(result.ToMinimalApiResult());
            return;
        }

        await HttpContext.Response.SendOkAsync(result.Value, cancellation: cancellationToken);
    }
}