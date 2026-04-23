using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Managers.Contracts;
using Managers.Features.Manager.CreateManager.Handler;
using Managers.Features.Manager.GetManagerDetails.Handler;
using Mediator;
using Microsoft.AspNetCore.Builder;

namespace Managers.Features.Manager.GetManagerDetails.Endpoint;



public class GetManagerDetails : Endpoint<GetManagerDetailsRequest, Result<ManagerDetailsDto>>
{
    private readonly IMediator _mediator;

    public GetManagerDetails(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get($"/{Constants.ModuleName}/" + "{CognitoId}");
        Roles("manager", "tenant");
        Options(x => x.RequireRateLimiting("per-user"));
    }

    public override async Task HandleAsync(GetManagerDetailsRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetManagerDetailsQuery(
            request.CognitoId
            );
        
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            await HttpContext.Response.SendAsync(result.Value, cancellation: cancellationToken);
        }
        else
        {
            await HttpContext.Response.SendResultAsync(result.ToMinimalApiResult());
            
        }


    }
}