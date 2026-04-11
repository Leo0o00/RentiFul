using Ardalis.Result;
using FluentValidation;
using Leases.Contracts;
using Mediator;
using SharedKernel;

namespace Leases.Features.Property.GetPropertyLeases.Handler;

public record GetPropertyLeasesQuery(
    string PropertyId
    ) : IRequest<Result<PropertyLeasesDto>>;

public class GetPropertyLeasesQueryValidator : AbstractValidator<GetPropertyLeasesQuery>
{
    public GetPropertyLeasesQueryValidator()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty()
            .WithMessage("PropertyId is required")
            .MustBeUuid();
    }
}