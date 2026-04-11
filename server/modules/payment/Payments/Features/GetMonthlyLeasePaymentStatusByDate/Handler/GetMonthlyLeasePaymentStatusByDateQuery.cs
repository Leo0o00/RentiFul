using Ardalis.Result;
using FluentValidation;
using Mediator;
using Payments.Contracts;
using SharedKernel;

namespace Payments.Features.GetMonthlyLeasePaymentStatusByDate.Handler;

public record GetMonthlyLeasePaymentStatusByDateQuery(
    string LeaseId,
    string DueDate
    ) : IRequest<Result<PaymentStatusDto>>;

public class GetMonthlyLeasePaymentStatusByDateQueryValidator : AbstractValidator<GetMonthlyLeasePaymentStatusByDateQuery>
{
    public GetMonthlyLeasePaymentStatusByDateQueryValidator()
    {
        RuleFor(x => x.LeaseId)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MustBeUuid();
        RuleFor(x => x.DueDate)
            .Must(x => DateOnly.TryParse(x, out _)).WithMessage("{PropertyName} must be a valid date expression");
    }
}