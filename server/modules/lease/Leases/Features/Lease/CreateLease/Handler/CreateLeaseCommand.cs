using Ardalis.Result;
using FluentValidation;
using Leases.Contracts;
using Mediator;
using SharedKernel;

namespace Leases.Features.Lease.CreateLease.Handler;

public record CreateLeaseCommand(Guid PropertyId, Guid TenantCognitoId, DateTime StartDate, DateTime EndDate) : IRequest<Result<CreateLeaseResponseDto>>;

// public class CreateLeaseCommandValidator : AbstractValidator<CreateLeaseCommand>
// {
//     public CreateLeaseCommandValidator()
//     {
//         RuleFor(x => x.PropertyId)
//             .NotEmpty().WithMessage("{PropertyName} is required")
//             .MustBeUuid();
//         RuleFor(x => x.Status)
//             .Must(x =>
//                 !string.IsNullOrWhiteSpace(x) &&
//                 Enum.Parse<ApplicationStatus>(x.Trim(), true) == ApplicationStatus.Approved ||
//                 Enum.Parse<ApplicationStatus>(x.Trim(), true) == ApplicationStatus.Denied
//             )
//             .WithMessage("Invalid value for {PropertyName}");
//     }
// }