using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Leases.Contracts;
using Leases.Features.Tenant.GetLeasesForTenant.Handler;
using Mediator;
using Microsoft.AspNetCore.Builder;

namespace Leases.Features.Tenant.GetLeasesForTenant.Endpoint;

public class GetLeasesForTenant : Endpoint<GetLeasesForTenantRequest, LeasesResponseDto>
{
    private readonly IMediator _mediator;

    public GetLeasesForTenant(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/tenants/{CognitoId}/properties/{PropertyId}" + $"/{Constants.ModuleName}");
        Roles("tenant", "manager");
        Options(x => x.RequireRateLimiting("per-user"));
    }

    public override async Task HandleAsync(GetLeasesForTenantRequest request, CancellationToken ct)
    {
        var query = new GetLeasesForTenantQuery(
            CognitoId: request.CognitoId,
            PropertyId: request.PropertyId);

        var result = await _mediator.Send(query, ct);

        if (result.IsSuccess)
        {
            await HttpContext.Response.SendOkAsync(result.Value,
                cancellation: ct
            );
        }
        else
        {
            await HttpContext.Response.SendResultAsync(result.ToMinimalApiResult());
        }
    }
}