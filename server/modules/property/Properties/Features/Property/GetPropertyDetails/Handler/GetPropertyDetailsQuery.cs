using Ardalis.Result;
using FluentValidation;
using Mediator;
using Properties.Contracts;

namespace Properties.Features.Property.GetPropertyDetails.Handler;

public record GetPropertyDetailsQuery(Guid PropertyId) : IRequest<Result<PropertyDetailsDto>>;

public class GetPropertyDetailsQueryValidator : AbstractValidator<GetPropertyDetailsQuery>
{
}