using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Properties.Contracts;
using Properties.Features.Property.GetPropertyMinInfo.Handler;

namespace Properties.Features.Property.GetPropertyMinInfo.Endpoint;

public class GetPropertyMinInfo : Endpoint<GetPropertyMinInfoRequest, Result<PropertyMinInfoDto>>
{
    private readonly IMediator _mediator;

    public GetPropertyMinInfo(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get($"/{Constants.ModuleName}/" + "{PropertyId}/minimal-information");
        Roles("manager", "tenant");
        AllowAnonymous();
        Options(x => x.RequireRateLimiting("per-user"));
    }

    public override async Task HandleAsync(GetPropertyMinInfoRequest request, CancellationToken ct)
    {
        var query = new GetPropertyMinInfoQuery(
            Guid.Parse(request.PropertyId)
        );

        var result = await _mediator.Send(query, ct);

        if (result.IsSuccess)
        {
            await HttpContext.Response.SendAsync(result.Value, cancellation: ct);
        }
        else
        {
            await HttpContext.Response.SendResultAsync(result.ToMinimalApiResult());

        }


    }
}