using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Managers.Features.Manager.UpdateManager.Handler;
using Mediator;
using Microsoft.AspNetCore.Builder;

namespace Managers.Features.Manager.UpdateManager.Endpoint;



public class UpdateManager : Endpoint<UpdateManagerRequest>
{
    private readonly IMediator _mediator;

    public UpdateManager(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put($"/{Constants.ModuleName}/" + "{CognitoId}");
        Roles("manager");
        Options(x => x.RequireRateLimiting("per-user"));
    }

    public override async Task HandleAsync(UpdateManagerRequest request, CancellationToken cancellationToken = default)
    {
        var command = new UpdateManagerCommand(
            CognitoId: request.CognitoId,
            Name: request.Name,
            Email: request.Email,
            PhoneNumber: request.PhoneNumber
            );
        
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            await HttpContext.Response.SendOkAsync(cancellation: cancellationToken);
        }
        else
        {
            await HttpContext.Response.SendResultAsync(result.ToMinimalApiResult());
            
        }
    }
}