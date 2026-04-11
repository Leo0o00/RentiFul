using Ardalis.Result;
using FluentValidation;
using Mediator;
using Properties.Contracts;
using Properties.Features.Property.GetAllPropertiesPaged.Handler;
using SharedKernel;

namespace Properties.Features.Manager.GetManagerProperties.Handler;

public record GetManagerPropertiesQuery(
    string CognitoId
) : IRequest<Result<PropertiesListDto>>;

public class GetManagerPropertiesQueryValidator : AbstractValidator<GetManagerPropertiesQuery>
{
    public GetManagerPropertiesQueryValidator()
    {
        RuleFor(x => x.CognitoId)
            .NotEmpty()
            .WithMessage("CognitoId is required")
            .MustBeUuid();
    }
}