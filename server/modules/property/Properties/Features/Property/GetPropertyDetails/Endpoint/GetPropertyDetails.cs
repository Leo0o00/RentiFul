using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Properties.Contracts;
using Properties.Features.Property.GetPropertyDetails.Handler;

namespace Properties.Features.Property.GetPropertyDetails.Endpoint;

public class GetPropertyDetails : Endpoint<GetPropertyDetailsRequest, Result<PropertyResponseDto>>
{
    private readonly IMediator _mediator;

    public GetPropertyDetails(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get($"/{Constants.ModuleName}/" + "{PropertyId}");
        AllowAnonymous();
        Options(x => x.RequireRateLimiting("per-user"));
    }

    public override async Task HandleAsync(GetPropertyDetailsRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetPropertyDetailsQuery(
            Guid.Parse(request.PropertyId)
        );

        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            await HttpContext.Response.SendAsync(result.Value, cancellation: cancellationToken);
        }
        else
        {
            await HttpContext.Response.SendResultAsync(result.ToMinimalApiResult());

        }


    }
}