using Ardalis.Result;
using FluentValidation;
using Mediator;
using Properties.Contracts;

namespace Properties.Features.Property.GetAllPropertiesPaged.Handler;

public record GetAllPropertiesPagedQuery(
    string? Location,
    decimal? PriceMin,
    decimal? PriceMax,
    string? Beds,
    string? Baths,
    string? PropertyType,
    double? SquareFeetMin,
    double? SquareFeetMax,
    string[]? Amenities,
    string? AvailableFrom,
    string[]? FavoriteIds,
    double? Latitude,
    double? Longitude
) : IRequest<Result<PropertiesListDto>>;

public class GetAllPropertiesPagedQueryValidator : AbstractValidator<GetAllPropertiesPagedQuery>
{
    public GetAllPropertiesPagedQueryValidator()
    {
    }
}