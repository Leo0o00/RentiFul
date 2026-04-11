using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Leases.Contracts;
using Leases.Features.Property.GetPropertyLeases.Handler;
using Mediator;

namespace Leases.Features.Property.GetPropertyLeases.Endpoint;

public class GetPropertyLeases : Endpoint<GetPropertyLeasesRequest, PropertyLeasesDto>
{
    private readonly IMediator _mediator;

    public GetPropertyLeases(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/properties/{PropertyId}" + $"/{Constants.ModuleName}");
        Roles("manager");
    }

    public override async Task HandleAsync(GetPropertyLeasesRequest request, CancellationToken ct)
    {
        var query = new GetPropertyLeasesQuery(
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