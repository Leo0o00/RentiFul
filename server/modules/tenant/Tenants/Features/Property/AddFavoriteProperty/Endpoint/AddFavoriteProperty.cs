using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Mediator;
using Tenants.Features.Property.AddFavoriteProperty.Handler;

namespace Tenants.Features.Property.AddFavoriteProperty.Endpoint;



public class AddFavoriteProperty : Endpoint<AddFavoritePropertyRequest>
{
    private readonly IMediator _mediator;

    public AddFavoriteProperty(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post($"/{Constants.ModuleName}/" + "{CognitoId}/favorites/{PropertyId}");
          Roles("tenant");
    }

    public override async Task HandleAsync(AddFavoritePropertyRequest request, CancellationToken cancellationToken = default)
    {
        var command = new AddFavoritePropertyCommand(
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