using FastEndpoints;
using FluentValidation;

namespace Properties.Features.Property.GetAllPropertiesPaged.Endpoint;

public record GetAllPropertiesPagedRequest(
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

);

public class GetAllPropertiesPagedRequestValidator : Validator<GetAllPropertiesPagedRequest>
{
    public GetAllPropertiesPagedRequestValidator()
    {
        // Todo: Implementar validaciones para la request aqui
        RuleFor(p => p.AvailableFrom)
            .Must(s => DateTime.TryParse(s, out var _))
            .When(p => !string.IsNullOrEmpty(p.AvailableFrom))
            .WithMessage("AvailableFrom field must be a valid datetime string.");
    }
}