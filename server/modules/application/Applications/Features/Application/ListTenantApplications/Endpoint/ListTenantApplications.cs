using System.Security.Claims;
using Applications.Contracts;
using Applications.Features.Application.ListManagerApplications.Handler;
using Applications.Features.Application.ListTenantApplications.Handler;
using Ardalis.GuardClauses;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Builder;

namespace Applications.Features.Application.ListTenantApplications.Endpoint;

public class ListTenantApplications(IMediator mediator)
    : Endpoint<ListTenantApplicationsRequest, Result<ListApplicationsResponseDto>>
{
    public override void Configure()
    {
        Get($"/tenants/{Constants.ModuleName}");
        Roles("tenant");
        Options(x => x.RequireRateLimiting("per-user"));
    }

    public override async Task HandleAsync(ListTenantApplicationsRequest request, CancellationToken ct)
    {
        var userIdClaimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        Guard.Against.Null(userIdClaimValue, nameof(userIdClaimValue));

        var userId = Guid.Parse(userIdClaimValue);

        var query = new ListTenantApplicationsQuery(UserId: userId, Page: request.Page, Limit: request.Limit);

        var result = await mediator.Send(query, ct);

        if (result.IsSuccess)
        {
            await HttpContext.Response.SendOkAsync(result.Value, cancellation: ct);
        }
        else
        {
            await HttpContext.Response.SendResultAsync(result.ToMinimalApiResult());
        }

    }
}