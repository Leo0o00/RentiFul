using FastEndpoints;
using FluentValidation;

namespace Managers.Features.Manager.UpdateManager.Endpoint;

public class UpdateManagerRequest
{
    [RouteParam]
    public required string CognitoId {get;set;}
    
    public required string Name {get;set;}
    public required string Email {get;set;}
    public required string PhoneNumber {get;set;}
}
    


public class UpdateManagerRequestValidator : Validator<UpdateManagerRequest>
{
    public UpdateManagerRequestValidator()
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