using FastEndpoints;
using FluentValidation;

namespace Managers.Features.Manager.GetManagerDetails.Endpoint;

public record GetManagerDetailsRequest(string CognitoId);
    


public class GetManagerDetailsRequestValidator : Validator<GetManagerDetailsRequest>
{
    public GetManagerDetailsRequestValidator()
    {
        RuleFor(x => x.CognitoId)
            .NotEmpty()
            .WithMessage("CognitoId is required")
            .MustBeUuid();
    }
}