using Microsoft.EntityFrameworkCore;
using Properties.Contracts;
using Properties.Data.Helpers;
using Properties.Domain;

namespace Properties.Data.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly PropertyDbContext _context;
    private const int PageSize = 40;

    public PropertyRepository(PropertyDbContext context)
    {
        _context = context;
    }

    public IQueryable<Property> GetAllProperties(bool noTracking = true)
    {
        var entityDbSet = _context.Set<Property>();

        if (noTracking)
        {
            return entityDbSet.AsNoTracking();
        }

        return entityDbSet;
    }

    public IQueryable<Manager> GetAllManagers(bool noTracking = true)
    {
        var entityDbSet = _context.Set<Manager>();

        if (noTracking)
        {
            return entityDbSet.AsNoTracking();
        }

        return entityDbSet;
    }

    public async Task<Guid> CreateProperty(Property property)
    {
        var propertyCreated = await _context.Properties.AddAsync(property);
        await _context.SaveChangesAsync();

        return propertyCreated.Entity.Id;
    }

    public async Task<Property?> GetPropertyById(Guid propertyId, bool isTracking = false)
    {
        return await GetAllProperties(isTracking)
            .Include(p => p.PropertyLocation)
            .Include(p => p.PhotoKeys)
            .FirstOrDefaultAsync(p => p.Id == propertyId);
    }

    public async Task<bool> CheckManagerExistence(string managerCognitoId)
    {
        return await GetAllManagers()
            .Where(m => m.Id == Guid.Parse(managerCognitoId))
            .AnyAsync();
    }

    public async Task<Manager?> GetManager(string managerCognitoId)
    {
        return await GetAllManagers()
            .FirstOrDefaultAsync(m => m.Id == Guid.Parse(managerCognitoId));
    }

    #region Refactor this shit ASAP

    public async Task<PropertiesQueryResultDto> GetPropertiesByFilters(PropertiesQueryFilters propertiesQueryFilters)
    {
        IQueryable<Property> query =  GetAllProperties();
        query = PropertyQueryBuilder.ApplyFilters(query, propertiesQueryFilters);

        // Todo: Implementar la paginacion

        var total = await query.CountAsync();

        var rows = await query
            .OrderByDescending(p => p.PostedAt)
            .Take(PageSize)
            .Include(p => p.PropertyLocation)
            .Include(p => p.PhotoKeys)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.PricePerMonth,
                FirstPhotoKey = p.PhotoKeys.OrderBy(x => x.Id).Select(x => x.PhotoKey).FirstOrDefault(),
                p.IsPetsAllowed,
                p.IsParkingIncluded,
                p.Beds,
                p.Baths,
                p.SquareFeet,
                AverageRating = p.AverageRating ?? 0,
                NumberOfReviews = p.NumberOfReviews ?? 0,
                p.PropertyLocation.Address,
                p.PropertyLocation.City,
                Coords = p.PropertyLocation.Coordinates,
                p.ManagerId
            })
            .ToListAsync();

        var items = rows
            .Select(p => new PropertyDto()
            {
                Id = p.Id,
                Name = p.Name,
                PricePerMonth = p.PricePerMonth,
                PhotoKey = p.FirstPhotoKey ?? string.Empty,
                IsPetsAllowed = p.IsPetsAllowed,
                IsParkingIncluded = p.IsParkingIncluded,
                Beds = p.Beds,
                Baths = p.Baths,
                SquareFeet = p.SquareFeet,
                AverageRating = p.AverageRating,
                NumberOfReviews = p.NumberOfReviews,
                ManagerId = p.ManagerId,
                Location = new LocationDto
                {
                    Address = p.Address,
                    City = p.City,
                    Coordinates = new CoordinatesDto
                    {
                        Longitude = p.Coords.X,
                        Latitude = p.Coords.Y
                    }
                }
            })
            .ToList();

        return new PropertiesQueryResultDto
        {
            count = total,
            properties = items
        };
    }

    public async Task<PropertiesQueryResultDto> GetPropertiesByManagerId(string cognitoId)
    {
        var managerCognitoId = Guid.Parse(cognitoId);

        IQueryable<Property> query =  GetAllProperties()
            .Where(p => p.ManagerId == managerCognitoId);

        // Todo: Implementar la paginacion

        var total = await query.CountAsync();

        var rows = await query
            .OrderByDescending(p => p.PostedAt)
            .Take(PageSize)
            .Include(p => p.PropertyLocation)
            .Include(p => p.PhotoKeys)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.PricePerMonth,
                FirstPhotoKey = p.PhotoKeys.OrderBy(x => x.Id).Select(x => x.PhotoKey).FirstOrDefault(),
                p.IsPetsAllowed,
                p.IsParkingIncluded,
                p.Beds,
                p.Baths,
                p.SquareFeet,
                AverageRating = p.AverageRating ?? 0,
                NumberOfReviews = p.NumberOfReviews ?? 0,
                p.PropertyLocation.Address,
                p.PropertyLocation.City,
                Coords = p.PropertyLocation.Coordinates
            })
            .ToListAsync();

        var items = rows
            .Select(p => new PropertyDto()
            {
                Id = p.Id,
                Name = p.Name,
                PricePerMonth = p.PricePerMonth,
                PhotoKey = p.FirstPhotoKey ?? string.Empty,
                IsPetsAllowed = p.IsPetsAllowed,
                IsParkingIncluded = p.IsParkingIncluded,
                Beds = p.Beds,
                Baths = p.Baths,
                SquareFeet = p.SquareFeet,
                AverageRating = p.AverageRating,
                NumberOfReviews = p.NumberOfReviews,
                Location = new LocationDto
                {
                    Address = p.Address,
                    City = p.City,
                    Coordinates = new CoordinatesDto
                    {
                        Longitude = p.Coords.X,
                        Latitude = p.Coords.Y
                    }
                }
            })
            .ToList();

        return new PropertiesQueryResultDto
        {
            count = total,
            properties = items
        };
    }

    public async Task<PropertiesQueryResultDto> GetAllPropertiesInAnIdList(List<Guid> propertiesIdList)
    {
        IQueryable<Property> query = GetAllProperties()
            .Where(p => propertiesIdList.Contains(p.Id));

        // Todo: Implementar la paginacion

        var total = await query.CountAsync();

        var rows = await query
            .OrderByDescending(p => p.PostedAt)
            .Take(PageSize)
            .Include(p => p.PropertyLocation)
            .Include(p => p.PhotoKeys)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.PricePerMonth,
                FirstPhotoKey = p.PhotoKeys.OrderBy(x => x.Id).Select(x => x.PhotoKey).FirstOrDefault(),
                p.IsPetsAllowed,
                p.IsParkingIncluded,
                p.Beds,
                p.Baths,
                p.SquareFeet,
                AverageRating = p.AverageRating ?? 0,
                NumberOfReviews = p.NumberOfReviews ?? 0,
                p.PropertyLocation.Address,
                p.PropertyLocation.City,
                Coords = p.PropertyLocation.Coordinates
            })
            .ToListAsync();

        var items = rows
            .Select(p => new PropertyDto()
            {
                Id = p.Id,
                Name = p.Name,
                PricePerMonth = p.PricePerMonth,
                PhotoKey = p.FirstPhotoKey ?? string.Empty,
                IsPetsAllowed = p.IsPetsAllowed,
                IsParkingIncluded = p.IsParkingIncluded,
                Beds = p.Beds,
                Baths = p.Baths,
                SquareFeet = p.SquareFeet,
                AverageRating = p.AverageRating,
                NumberOfReviews = p.NumberOfReviews,
                Location = new LocationDto
                {
                    Address = p.Address,
                    City = p.City,
                    Coordinates = new CoordinatesDto
                    {
                        Longitude = p.Coords.X,
                        Latitude = p.Coords.Y
                    }
                }
            })
            .ToList();

        return new PropertiesQueryResultDto
        {
            count = total,
            properties = items
        };
    }

    #endregion Refactor this shit ASAP

    public async Task<PropertyMinInfoDto?> GetPropertyMinimalInfoById(Guid propertyId)
    {
        return await GetAllProperties()
            .Where(p => p.Id == propertyId)
            .Select(p => new PropertyMinInfoDto
            {
                Id = p.Id,
                Name = p.Name,
                PricePerMonth = p.PricePerMonth,
                SecurityDeposit = p.SecurityDeposit
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PropertyMinInfoDto?> GetPropertyInfoById(Guid propertyId)
    {
        return await GetAllProperties()
            .Include(p => p.PhotoKeys)
            .Include(p => p.PropertyLocation)
            .Where(p => p.Id == propertyId)
            .Select(p => new PropertyMinInfoDto
            {
                Id = p.Id,
                Name = p.Name,
                PricePerMonth = p.PricePerMonth,
                SecurityDeposit = p.SecurityDeposit,
                PhotoKey = p.PhotoKeys.Select(x => x.PhotoKey).FirstOrDefault() ?? string.Empty,
                Location = new LocationMinInfoDto
                {
                    City = p.PropertyLocation.City,
                    Country = p.PropertyLocation.Country,
                }
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}