using FastEndpoints;
using FluentValidation;
using SharedKernel;

namespace Payments.Features.GetMonthlyLeasePaymentStatusByDate.EndPoint;

public record GetMonthlyLeasePaymentStatusByDateRequest(
    string LeaseId,
    string DueDate
);

public class GetPropertyLeasesRequestValidator : Validator<GetMonthlyLeasePaymentStatusByDateRequest>
{
    public GetPropertyLeasesRequestValidator()
    {
        RuleFor(x => x.LeaseId)
            .NotEmpty().WithMessage("LeaseId is required")
            .MustBeUuid();
        RuleFor(x => x.DueDate)
            .Must(x => DateOnly.TryParse(x, out _)).WithMessage("{PropertyName} must be a valid date expression");
    }
}