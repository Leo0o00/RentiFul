using Applications.Contracts;
using Applications.Domain;
using Ardalis.Result;
using FluentValidation;
using Mediator;
using SharedKernel;

namespace Applications.Features.Application.CreateApplication.Handler;

public record CreateApplicationCommand(
    string ApplicationDate,
    string Status,
    string PropertyId,
    string TenantCognitoId,
    string? Message
) : IRequest<Result<CreateApplicationResponseDto>>;

public class CreateApplicationCommandValidator : AbstractValidator<CreateApplicationCommand>
{
    public CreateApplicationCommandValidator()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MustBeUuid();
        RuleFor(x => x.TenantCognitoId)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MustBeUuid();
        RuleFor(x => x.ApplicationDate)
            .Must(x => DateTime.TryParse(x, out _)).WithMessage("{PropertyName} must be a valid date expression");
        When(x => x.Message != null, () =>
        {
            RuleFor(x => x.Message)
                .Length(0, 1000).WithMessage("{PropertyName} may not exceed 1000 characters");

        });
        RuleFor(x => x.Status)
            .Must(x =>
                !string.IsNullOrWhiteSpace(x) &&
                Enum.Parse<ApplicationStatus>(x.Trim(), true) == ApplicationStatus.Pending
            )
            .WithMessage("{PropertyName} must be Pending");
    }
}