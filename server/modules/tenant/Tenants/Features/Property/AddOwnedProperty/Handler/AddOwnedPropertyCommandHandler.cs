using Ardalis.Result;
using Mediator;
using Tenants.Contracts;
using Tenants.Data.Repositories;
using Tenants.ExceptionMessages;

namespace Tenants.Features.Property.AddOwnedProperty.Handler;

public class AddOwnedPropertyCommandHandler : IRequestHandler<AddOwnedPropertyCommand, Result<AddOwnedPropertyResponse>>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IPropertyRepository _propertyRepository;

    public AddOwnedPropertyCommandHandler(ITenantRepository tenantRepository, IPropertyRepository propertyRepository)
    {
        _tenantRepository = tenantRepository;
        _propertyRepository = propertyRepository;
    }

    public async ValueTask<Result<AddOwnedPropertyResponse>> Handle(AddOwnedPropertyCommand request, CancellationToken cancellationToken)
    {
        var tenantCognitoIdString = request.TenantCognitoId.ToString();
        var tenant = await _tenantRepository.GetWithOwnedPropertiesByCognitoId(tenantCognitoIdString, false);
        if (tenant is null)
        {
            return Result.NotFound(TenantExceptionsMessages.TenantNotFound);
        }

        var property = await _propertyRepository.GetById(propertyId: request.PropertyId);
        if (property is null)
        {
            return Result.NotFound(PropertyExceptionsMessages.PropertyNotFound);
        }


        var updateOwnedPropertiesResult = tenant.AddPropertyToOwnedProperties(property);


        if (updateOwnedPropertiesResult.IsConflict())
        {
            return Result.Conflict(PropertyExceptionsMessages.PropertyAlreadyAddedToOwnedProperties);
        }

        await _tenantRepository.SaveChangesAsync();

        return Result.Success();

    }
}