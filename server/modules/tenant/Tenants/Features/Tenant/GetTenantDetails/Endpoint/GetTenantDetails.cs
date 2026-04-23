using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Tenants.Contracts;
using Tenants.Features.Tenant.CreateTenant.Handler;

namespace Tenants.Features.Tenant.GetTenantDetails.Endpoint;

public class GetTenantDetails : Endpoint<GetTenantDetailsRequest, Result<TenantDetailsDto>>
{
    private readonly IMediator _mediator;

    public GetTenantDetails(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get($"/{Constants.ModuleName}/" + "{CognitoId}");
        Roles("tenant");
        Options(x => x.RequireRateLimiting("per-user"));
    }

    public override async Task HandleAsync(GetTenantDetailsRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetTenantDetailsQuery(
            request.CognitoId
        );
        
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            await HttpContext.Response.SendAsync(result.Value);
        }
        else
        {
            await HttpContext.Response.SendResultAsync(result.ToMinimalApiResult());
            
        }



    }
}