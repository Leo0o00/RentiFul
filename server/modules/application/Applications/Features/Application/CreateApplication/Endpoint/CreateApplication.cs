using Applications.Features.Application.CreateApplication.Handler;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Applications.Features.Application.CreateApplication.Endpoint;

public class CreateApplication : Endpoint<CreateApplicationRequest>
{
    private readonly IMediator _mediator;

    public CreateApplication(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post($"/{Constants.ModuleName}");
        Roles("tenant");
        Options(x => x.RequireRateLimiting("per-user"));
    }

    public override async Task HandleAsync(CreateApplicationRequest request, CancellationToken ct)
    {
        var command = new CreateApplicationCommand(
            ApplicationDate: request.ApplicationDate,
            Status: request.Status,
            PropertyId: request.PropertyId,
            TenantCognitoId: request.TenantCognitoId,
            Message: request.Message
            );

        var result = await _mediator.Send(command, ct);

        if (result.IsSuccess)
        {
            await HttpContext.Response.SendCreatedAtAsync($"/{Constants.ModuleName}/{result.Value.ApplicationId}", result.Value, cancellation: ct);
        }
        else
        {
            await HttpContext.Response.SendResultAsync(result.ToMinimalApiResult());
        }

    }
}