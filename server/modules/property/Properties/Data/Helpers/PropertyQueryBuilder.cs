using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Properties.Contracts;
using Properties.Domain;

namespace Properties.Data.Helpers;

public static class PropertyQueryBuilder
{
    public static IQueryable<Property>  ApplyFilters(IQueryable<Property> query, PropertiesQueryFilters filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Location))
        {
            var location = filter.Location.Trim();

            query = query
                .Include(p => p.PropertyLocation)
                .Where(p => p.PropertyLocation.City == location);
        }

        // Favorites
        if (filter.FavoriteIds is { Length: > 0 })
        {
            var ids = filter.FavoriteIds
                .Select(x => Guid.TryParse(x, out var g) ? (Guid?)g : null)
                .Where(x => x.HasValue)
                .Select(x => x!.Value)
                .ToArray();

            if (ids.Length > 0)
                query = query.Where(p => ids.Contains(p.Id));
        }

        // Price
        if (filter.PriceMin.HasValue)
            query = query.Where(p => p.PricePerMonth >= filter.PriceMin.Value);

        if (filter.PriceMax.HasValue)
            query = query.Where(p => p.PricePerMonth <= filter.PriceMax.Value);

        if (FilterParsing.TryParseMinInt(filter.Beds, out var bedsMin) && bedsMin.HasValue)
            query = query.Where(p => p.Beds >= bedsMin.Value);

        if (FilterParsing.TryParseMinDouble(filter.Baths, out var bathsMin) && bathsMin.HasValue)
            query = query.Where(p => p.Baths >= bathsMin.Value);

        if (filter.SquareFeetMin.HasValue)
            query = query.Where(p => p.SquareFeet >= filter.SquareFeetMin.Value);

        if (filter.SquareFeetMax.HasValue)
            query = query.Where(p => p.SquareFeet <= filter.SquareFeetMax.Value);

        if (!string.IsNullOrWhiteSpace(filter.PropertyType) &&
            FilterParsing.TryParseEnumIgnoreCase<PropertyType>(filter.PropertyType, out var pt))
        {
            query = query.Where(p => p.Type == pt);
        }

        if (filter.Amenities is { Length: > 0 })
        {
            var amenities = filter.Amenities
                .Select(a => FilterParsing.TryParseEnumIgnoreCase<Amenity>(a, out var e) ? (Amenity?)e : null)
                .Where(x => x.HasValue)
                .Select(x => x!.Value)
                .ToList();

            if (amenities.Count > 0)
            {
                // Overlap: any requested amenity matches
                // query = query.Where(p => p.Amenities.Any(a => amenities.Contains(a)));

                // All required: must contain all
                // query = query.Where(p => amenities.All(a => p.Amenities.Contains(a)));
                foreach (var amenity in amenities)
                {
                    query = query.Where(p => p.Amenities.Contains(amenity));
                }
            }
        }

        if (FilterParsing.TryParseDate(filter.AvailableFrom, out var availableFrom) && availableFrom.HasValue)
        {
            query = query.Where(p => p.PostedAt != null && p.PostedAt >= availableFrom.Value);
        }

        if (filter.Latitude.HasValue && filter.Longitude.HasValue)
        {
            var userPoint = new Point(filter.Longitude.Value, filter.Latitude.Value) { SRID = 4326 };

            const double radiusKm = 1000;
            var degrees = radiusKm / 111d;

            query = query.Where(p =>
                EF.Functions.IsWithinDistance(
                    p.PropertyLocation.Coordinates,
                    userPoint,
                    degrees,
                    true)
                );
        }

        return query;
    }
}