namespace Properties.Data.Helpers;

public record PropertiesQueryFilters(
    string? Location = null,
    decimal? PriceMin = null,
    decimal? PriceMax = null,
    string? Beds = null,
    string? Baths = null,
    string? PropertyType = null,
    double? SquareFeetMin = null,
    double? SquareFeetMax = null,
    string[]? Amenities = null,
    string? AvailableFrom = null,
    string[]? FavoriteIds = null,
    double? Latitude = null,
    double? Longitude = null
    );