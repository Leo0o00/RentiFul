using Ardalis.Result;
using FluentValidation;
using Mediator;

namespace Tenants.Features.Tenant.UpdateTenant.Handler;

public record UpdateTenantCommand(
    string CognitoId,
    string Name,
    string Email,
    string PhoneNumber
) : IRequest<Result>;

public class UpdateTenantCommandValidator : AbstractValidator<UpdateTenantCommand>
{
    public UpdateTenantCommandValidator()
    {
        RuleFor(x => x.CognitoId)
            .NotEmpty()
            .WithMessage("CognitoId is required")
            .MustBeUuid();
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(256)
            .WithMessage("Name cannot exceed 256 characters length")
            .MinimumLength(3)
            .WithMessage("Name most exceed 3 characters length");
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .MaximumLength(256)
            .WithMessage("Email cannot exceed 256 characters length")
            .EmailAddress();
        RuleFor(x => x.PhoneNumber)
            .NotNull()
            .WithMessage("Phone number is required")
            .MaximumLength(256)
            .WithMessage("Phone number cannot exceed 256 characters length");
    }
}