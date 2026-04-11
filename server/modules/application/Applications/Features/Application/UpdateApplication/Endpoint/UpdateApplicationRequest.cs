using Applications.Domain;
using Applications.Features.Application.CreateApplication.Endpoint;
using FastEndpoints;
using FluentValidation;
using SharedKernel;

namespace Applications.Features.Application.UpdateApplication.Endpoint;

public class UpdateApplicationRequest
{
    [RouteParam]
    public string ApplicationId { get; set; }
    public string Status { get; set; }

}

public class UpdateApplicationRequestValidator : Validator<UpdateApplicationRequest>
{
    public UpdateApplicationRequestValidator()
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