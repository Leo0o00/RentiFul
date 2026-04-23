using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Properties.Contracts;
using Properties.Features.Manager.GetManagerProperties.Endpoint;
using Properties.Features.Manager.GetManagerProperties.Handler;
using Properties.Features.Property.GetAllPropertiesInIdList.Handler;

namespace Properties.Features.Property.GetAllPropertiesInIdList.Endpoint;



public class GetAllPropertiesInIdList : Endpoint<GetAllPropertiesInIdListRequest, Result<PropertiesListDto>>
{
    private readonly IMediator _mediator;

    public GetAllPropertiesInIdList(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post($"/{Constants.ModuleName}/in-id-list/");
        Roles("tenant");
        Options(x => x.RequireRateLimiting("per-user"));
    }

    public override async Task HandleAsync(GetAllPropertiesInIdListRequest request, CancellationToken ct)
    {
        if (request.PropertiesId.Count == 0)
        {
            var response = Result.NotFound("No properties found");
            await HttpContext.Response.SendResultAsync(response.ToMinimalApiResult());
            return;
        }
        var query = new GetAllPropertiesInIdListQuery(
            PropertiesId: request.PropertiesId
            );
        
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