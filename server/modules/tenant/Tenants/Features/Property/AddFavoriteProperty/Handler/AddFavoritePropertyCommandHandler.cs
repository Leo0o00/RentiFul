using Ardalis.Result;
using Mediator;
using Tenants.Contracts;
using Tenants.Data.Repositories;
using Tenants.ExceptionMessages;
using Tenants.Features.Tenant.CreateTenant.Handler;

namespace Tenants.Features.Property.AddFavoriteProperty.Handler;

public class AddFavoritePropertyCommandHandler : IRequestHandler<AddFavoritePropertyCommand, Result<Guid>>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IPropertyRepository _propertyRepository;

    public AddFavoritePropertyCommandHandler(ITenantRepository tenantRepository, IPropertyRepository propertyRepository)
    {
        _tenantRepository = tenantRepository;
        _propertyRepository = propertyRepository;
    }
    
    public async ValueTask<Result<Guid>> Handle(AddFavoritePropertyCommand request, CancellationToken cancellationToken)
    {
        // var tenant = await _tenantRepository.GetByIdDetailsWithFavoriteProperties(request.CognitoId, false);
        var tenant = await _tenantRepository.GetWithFavoritesByCognitoId(request.CognitoId, false);
        if (tenant == null)
        {
            return Result.NotFound(TenantExceptionsMessages.TenantNotFound);
        }

        var property = await _propertyRepository.GetById(propertyId: request.PropertyId);
        if (property == null)
        {
            return Result.NotFound(PropertyExceptionsMessages.PropertyNotFound);
        }

        
        var updateFavoritesResult = tenant.AddPropertyToFavorites(property);
        
        
        if (updateFavoritesResult.IsConflict())
        {
            return Result.Conflict(PropertyExceptionsMessages.PropertyAlreadyAddedToFavorites);
        }

        // Console.WriteLine("{0} has been made in database.");

        await _tenantRepository.SaveChangesAsync();
        
        return Guid.Parse(tenant.CognitoId);



    }
}