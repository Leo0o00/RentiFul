using Ardalis.Result;
using FluentValidation;
using Mediator;
using Properties.Contracts;

namespace Properties.Features.Property.GetPropertyMinInfo.Handler;

public record GetPropertyMinInfoQuery(Guid PropertyId) : IRequest<Result<PropertyMinInfoDto>>;

public class GetPropertyMinInfoQueryValidator : AbstractValidator<GetPropertyMinInfoQuery>
{
}