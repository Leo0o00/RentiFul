using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Mediator;
using Tenants.Features.Property.AddFavoriteProperty.Endpoint;
using Tenants.Features.Property.AddFavoriteProperty.Handler;
using Tenants.Features.Property.RemoveFavoriteProperty.Handler;

namespace Tenants.Features.Property.RemoveFavoriteProperty.Endpoint;



public class RemoveFavoriteProperty : Endpoint<RemoveFavoritePropertyRequest>
{
    private readonly IMediator _mediator;

    public RemoveFavoriteProperty(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete($"/{Constants.ModuleName}/" + "{CognitoId}/favorites/{PropertyId}");
        Roles("tenant");
    }

    public override async Task HandleAsync(RemoveFavoritePropertyRequest request, CancellationToken cancellationToken = default)
    {
        var command = new RemoveFavoritePropertyCommand(
            request.CognitoId,
            request.PropertyId
            );
        
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            await HttpContext.Response.SendOkAsync(result.Value, cancellation: cancellationToken);
        }
        else
        {
            await HttpContext.Response.SendResultAsync(result.ToMinimalApiResult());
            
        }

    }
}