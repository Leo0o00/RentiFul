using FastEndpoints;
using FluentValidation;
using SharedKernel;

namespace Properties.Features.Manager.GetManagerProperties.Endpoint;

public record GetManagerPropertiesRequest(
    string CognitoId
);
    


public class GetManagerPropertiesRequestValidator : Validator<GetManagerPropertiesRequest>
{
    public GetManagerPropertiesRequestValidator()
    {
        RuleFor(x => x.CognitoId)
            .NotEmpty()
            .WithMessage("CognitoId is required")
            .MustBeUuid();
    }
}