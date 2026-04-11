using Ardalis.Result;
using FluentValidation;
using Mediator;
using Properties.Contracts;

namespace Properties.Features.Property.GetAllPropertiesInIdList.Handler;

public record GetAllPropertiesInIdListQuery(
    List<string> PropertiesId
) : IRequest<Result<PropertiesListDto>>;

public class GetAllPropertiesInIdListQueryValidator : AbstractValidator<GetAllPropertiesInIdListQuery>
{
    public GetAllPropertiesInIdListQueryValidator()
    {
    }
}