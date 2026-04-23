using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Tenants.Contracts;
using Tenants.Features.Tenant.CreateTenant.Handler;

namespace Tenants.Features.Tenant.CreateTenant.Endpoint;



public class CreateTenant : Endpoint<CreateTenantRequest, Result<CreateTenantResponseDto>>
{
    private readonly IMediator _mediator;

    public CreateTenant(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post($"/{Constants.ModuleName}");
        Roles("tenant");
        Options(x => x.RequireRateLimiting("per-user"));
    }

    public override async Task HandleAsync(CreateTenantRequest request, CancellationToken cancellationToken = default)
    {
        var command = new CreateTenantCommand(
            request.CognitoId,
            request.Name,
            request.Email,
            request.PhoneNumber
            );
        
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            await HttpContext.Response.SendCreatedAtAsync(
                $"/{Constants.ModuleName}/{request.CognitoId}", 
                responseBody: result.Value,
                cancellation: cancellationToken
                );
        }
        else
        {
            await HttpContext.Response.SendResultAsync(result.ToMinimalApiResult());
        }

    }
}