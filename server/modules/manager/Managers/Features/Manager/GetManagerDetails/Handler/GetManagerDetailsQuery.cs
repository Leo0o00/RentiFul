using Ardalis.Result;
using FluentValidation;
using Managers.Contracts;
using Mediator;

namespace Managers.Features.Manager.GetManagerDetails.Handler;

public record GetManagerDetailsQuery(
    string CognitoId
) : IRequest<Result<ManagerDetailsDto>>;

public class GetManagerDetailsQueryValidator : AbstractValidator<GetManagerDetailsQuery>
{
    public GetManagerDetailsQueryValidator()
    {
        RuleFor(x => x.CognitoId)
            .NotEmpty()
            .WithMessage("CognitoId is required")
            .MustBeUuid();
    }
}