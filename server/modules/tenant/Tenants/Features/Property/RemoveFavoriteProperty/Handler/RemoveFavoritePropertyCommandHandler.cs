using Ardalis.Result;
using Mediator;
using Tenants.Data.Repositories;
using Tenants.ExceptionMessages;
using Tenants.Features.Property.AddFavoriteProperty.Handler;

namespace Tenants.Features.Property.RemoveFavoriteProperty.Handler;

public class RemoveFavoritePropertyCommandHandler : IRequestHandler<RemoveFavoritePropertyCommand, Result<Guid>>
{
    private readonly ITenantRepository _tenantRepository;

    public RemoveFavoritePropertyCommandHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }
    
    public async ValueTask<Result<Guid>> Handle(RemoveFavoritePropertyCommand request, CancellationToken cancellationToken)
    {
        // var tenant = await _tenantRepository.GetByIdDetailsWithFavoriteProperties(request.CognitoId, false);
        var tenant = await _tenantRepository.GetWithFavoritesByCognitoId(request.CognitoId, false);
        if (tenant == null)
        {
            return Result.NotFound(TenantExceptionsMessages.TenantNotFound);
        }
        
        var updateFavoritesResult = tenant.RemovePropertyFromFavorites(propertyId: Guid.Parse(request.PropertyId));
        
        
        if (updateFavoritesResult.IsNotFound())
        {
            return Result.NotFound(PropertyExceptionsMessages.PropertyNotFoundInFavorites);
        }

        await _tenantRepository.SaveChangesAsync();
        
        // Console.WriteLine("{0} has been made in database.");
        
        return Guid.Parse(tenant.CognitoId);


    }
}