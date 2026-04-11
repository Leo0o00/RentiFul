using FastEndpoints;
using FluentValidation;
using SharedKernel;

namespace Properties.Features.Property.GetPropertyDetails.Endpoint;

public record GetPropertyDetailsRequest(string PropertyId);

public class GetPropertyDetailsRequestValidator : Validator<GetPropertyDetailsRequest>
{
    public GetPropertyDetailsRequestValidator()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty()
            .WithMessage("PropertyId is required")
            .MustBeUuid();
    }
}