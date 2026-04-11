using Applications.Contracts;
using Applications.Domain;
using Ardalis.Result;
using FluentValidation;
using Mediator;
using SharedKernel;

namespace Applications.Features.Application.UpdateApplication.Handler;

public record UpdateApplicationCommand(
    string ApplicationId,
    string Status
) : IRequest<Result>;

public class UpdateApplicationCommandValidator : AbstractValidator<UpdateApplicationCommand>
{
    public UpdateApplicationCommandValidator()
    {
        RuleFor(x => x.ApplicationId)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MustBeUuid();
        RuleFor(x => x.Status)
            .Must(x =>
                !string.IsNullOrWhiteSpace(x) &&
                Enum.Parse<ApplicationStatus>(x.Trim(), true) == ApplicationStatus.Approved ||
                Enum.Parse<ApplicationStatus>(x.Trim(), true) == ApplicationStatus.Denied
            )
            .WithMessage("Invalid value for {PropertyName}");
    }
}