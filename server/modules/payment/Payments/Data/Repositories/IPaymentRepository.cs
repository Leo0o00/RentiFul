using Ardalis.Result;
using Payments.Contracts;

namespace Payments.Data.Repositories;

public interface IPaymentRepository
{
    Task<bool> CheckLeaseExistence(Guid leaseId);


    Task<PaymentStatusDto> GetLeasePaymentStatusByDate(Guid leaseId, DateOnly dueDate);
}