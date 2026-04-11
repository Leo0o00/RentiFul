using Ardalis.Result;
using MassTransit;
using Mediator;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Events;
using Tenants.Contracts;
using Tenants.Data.Repositories;
using Tenants.ExceptionMessages;
using Tenants.Features.Tenant.CreateTenant.Handler;

namespace Tenants.Features.Tenant.UpdateTenant.Handler;

public class UpdateTenantCommandHandler : IRequestHandler<UpdateTenantCommand, Result>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IBus _bus;

    public UpdateTenantCommandHandler(ITenantRepository tenantRepository, IBus bus)
    {
        _tenantRepository = tenantRepository;
        _bus = bus;
    }
    
    public async ValueTask<Result> Handle(UpdateTenantCommand request, CancellationToken ct)
    {
        var tenant = await _tenantRepository.GetByCognitoId(
            cognitoId: request.CognitoId,
            noTracking: false
        );

        if (tenant is null)
        {
            return Result.NotFound();
        }

        if (tenant.Email != request.Email)
        {
            var tenantIsNotAvailable = await _tenantRepository.CheckTenantEmailAvailability(
                email: request.Email
            );
            
            if (tenantIsNotAvailable)
            {
                return Result.Conflict(TenantExceptionsMessages.TenantNotAvailable);
            }
            
        }
        
        tenant.Update(
            name: request.Name,
            email: request.Email,
            phoneNumber: request.PhoneNumber);
        
        await  _tenantRepository.SaveChangesAsync();

        await _bus.Publish(new TenantUpdated
        {
            TenantId = Guid.Parse(tenant.CognitoId),
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        }, cancellationToken: ct);

        return Result.Success();
    }
}