using Applications.Features.Application.CreateApplication.Endpoint;
using Applications.Features.Application.CreateApplication.Handler;
using Applications.Features.Application.UpdateApplication.Handler;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Mediator;

namespace Applications.Features.Application.UpdateApplication.Endpoint;

public class UpdateApplication : Endpoint<UpdateApplicationRequest>
{
    private readonly IMediator _mediator;

    public UpdateApplication(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post($"/{Constants.ModuleName}/" + "{ApplicationId}/status");
        Roles("manager");
    }

    public override async Task HandleAsync(UpdateApplicationRequest request, CancellationToken ct)
    {
        var command = new UpdateApplicationCommand(
            ApplicationId: request.ApplicationId,
            Status: request.Status
            );

        var result = await _mediator.Send(command, ct);

        if (result.IsSuccess)
        {
            await HttpContext.Response.SendOkAsync(cancellation: ct);
        }
        else
        {
            await HttpContext.Response.SendResultAsync(result.ToMinimalApiResult());
        }

    }
}