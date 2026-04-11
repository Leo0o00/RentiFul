using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Managers.Features.Manager.CreateManager.Handler;
using Mediator;
using Microsoft.AspNetCore.Http;

namespace Managers.Features.Manager.CreateManager.Endpoint;



public class CreateManager : Endpoint<CreateManagerRequest>
{
    private readonly IMediator _mediator;

    public CreateManager(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post($"/{Constants.ModuleName}");
        Roles("manager");
    }

    public override async Task HandleAsync(CreateManagerRequest request, CancellationToken cancellationToken = default)
    {
        var command = new CreateManagerCommand(
            request.CognitoId,
            request.Name,
            request.Email,
            request.PhoneNumber
            );
        
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            await HttpContext.Response.SendCreatedAtAsync($"/{Constants.ModuleName}/{request.CognitoId}",cancellation: cancellationToken);
        }
        else
        {
            await HttpContext.Response.SendResultAsync(result.ToMinimalApiResult());
        }

    }
}