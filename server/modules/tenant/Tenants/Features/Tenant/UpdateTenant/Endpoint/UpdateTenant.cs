using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Mediator;
using Tenants.Features.Tenant.CreateTenant.Handler;
using Tenants.Features.Tenant.GetTenantDetails.Endpoint;
using Tenants.Features.Tenant.UpdateTenant.Handler;

namespace Tenants.Features.Tenant.UpdateTenant.Endpoint;

public class UpdateTenant : Endpoint<UpdateTenantRequest>
{
    private readonly IMediator _mediator;

    public UpdateTenant(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put($"/{Constants.ModuleName}/" + "{CognitoId}");
        Roles("tenant");
    }

    public override async Task HandleAsync(UpdateTenantRequest request, CancellationToken cancellationToken = default)
    {
        var command = new UpdateTenantCommand(
            request.CognitoId,
            request.Name,
            request.Email,
            request.PhoneNumber
        );
        
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            await HttpContext.Response.SendOkAsync(cancellationToken);
        }
        else
        {
            await HttpContext.Response.SendResultAsync(result.ToMinimalApiResult());
            
        }



    }
}