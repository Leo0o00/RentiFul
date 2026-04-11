using System.Security.Claims;
using Applications.Contracts;
using Applications.Features.Application.ListManagerApplications.Handler;
using Ardalis.GuardClauses;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Mediator;

namespace Applications.Features.Application.ListManagerApplications.Endpoint;

public class ListManagerApplications(IMediator mediator)
    : Endpoint<ListManagerApplicationsRequest, Result<ListApplicationsResponseDto>>
{
    public override void Configure()
    {
        Get($"/managers/{Constants.ModuleName}");
        Roles("manager");
    }

    public override async Task HandleAsync(ListManagerApplicationsRequest request, CancellationToken ct)
    {
        var userIdClaimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        Guard.Against.Null(userIdClaimValue, nameof(userIdClaimValue));

        var userId = Guid.Parse(userIdClaimValue);

        var query = new ListManagerApplicationsQuery(UserId: userId, Page: request.Page, Limit: request.Limit);

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