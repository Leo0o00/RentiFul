using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Payments.Contracts;
using Payments.Features.GetMonthlyLeasePaymentStatusByDate.Handler;

namespace Payments.Features.GetMonthlyLeasePaymentStatusByDate.EndPoint;

public class GetMonthlyLeasePaymentStatusByDate : Endpoint<GetMonthlyLeasePaymentStatusByDateRequest, PaymentStatusDto>
{
    private readonly IMediator _mediator;

    public GetMonthlyLeasePaymentStatusByDate(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/leases/{LeaseId}/payment-status/{DueDate}");
        Roles("manager");
        Options(x => x.RequireRateLimiting("per-user"));
    }

    public override async Task HandleAsync(GetMonthlyLeasePaymentStatusByDateRequest request, CancellationToken ct)
    {
        var query = new GetMonthlyLeasePaymentStatusByDateQuery(
            LeaseId: request.LeaseId,
            DueDate: request.DueDate);

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