using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Mediator;
using Properties.Contracts;
using Properties.Features.Manager.GetManagerProperties.Handler;
using Properties.Features.Property.GetAllPropertiesPaged.Endpoint;

namespace Properties.Features.Manager.GetManagerProperties.Endpoint;



public class GetManagerProperties : Endpoint<GetManagerPropertiesRequest, Result<PropertiesListDto>>
{
    private readonly IMediator _mediator;

    public GetManagerProperties(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get( "/managers/{CognitoId}/" + $"{Constants.ModuleName}");
        Roles("manager");
    }

    public override async Task HandleAsync(GetManagerPropertiesRequest request, CancellationToken ct)
    {
        var query = new GetManagerPropertiesQuery(
            CognitoId: request.CognitoId
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