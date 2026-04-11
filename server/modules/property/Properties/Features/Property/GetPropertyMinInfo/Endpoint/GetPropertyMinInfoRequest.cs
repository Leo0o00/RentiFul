using FastEndpoints;
using FluentValidation;
using SharedKernel;

namespace Properties.Features.Property.GetPropertyMinInfo.Endpoint;

public record GetPropertyMinInfoRequest(string PropertyId);

public class GetPropertyMinInfoRequestValidator : Validator<GetPropertyMinInfoRequest>
{
    public GetPropertyMinInfoRequestValidator()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty()
            .WithMessage("PropertyId is required")
            .MustBeUuid();
    }
}