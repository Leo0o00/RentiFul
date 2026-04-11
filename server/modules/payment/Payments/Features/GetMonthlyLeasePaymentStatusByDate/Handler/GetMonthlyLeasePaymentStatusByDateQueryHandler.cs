using Ardalis.Result;
using Mediator;
using Payments.Contracts;
using Payments.Data.Repositories;

namespace Payments.Features.GetMonthlyLeasePaymentStatusByDate.Handler;

public class GetMonthlyLeasePaymentStatusByDateQueryHandler : IRequestHandler<GetMonthlyLeasePaymentStatusByDateQuery, Result<PaymentStatusDto>>
{
    private readonly IPaymentRepository _paymentRepository;

    public GetMonthlyLeasePaymentStatusByDateQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async ValueTask<Result<PaymentStatusDto>> Handle(GetMonthlyLeasePaymentStatusByDateQuery request,
        CancellationToken ct)
    {
        var leaseId = Guid.Parse(request.LeaseId);
        var dueDate = DateOnly.Parse(request.DueDate);

        var leaseExist = await _paymentRepository.CheckLeaseExistence(leaseId);

        if (!leaseExist)
        {
            return Result.NotFound("Lease not found with the provided id.");
        }

        var result = await _paymentRepository.GetLeasePaymentStatusByDate(leaseId, dueDate);

        return result;

    }
}