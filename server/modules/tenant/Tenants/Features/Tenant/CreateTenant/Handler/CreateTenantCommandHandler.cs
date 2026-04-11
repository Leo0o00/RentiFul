using Ardalis.Result;
using FastEndpoints;
using MassTransit;
using Mediator;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Events;
using Tenants.Contracts;
using Tenants.Domain;
using Tenants.Data.Repositories;
using Tenants.ExceptionMessages;

namespace Tenants.Features.Tenant.CreateTenant.Handler;

public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, Result<CreateTenantResponseDto>>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly CreateTenantMapper _mapper;
    private readonly IBus _bus;

    public CreateTenantCommandHandler(ITenantRepository tenantRepository, CreateTenantMapper mapper, IBus bus)
    {
        _tenantRepository = tenantRepository;
        _mapper = mapper;
        _bus = bus;
    }
    
    public async ValueTask<Result<CreateTenantResponseDto>> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenantIsNotAvailable = await _tenantRepository.CheckTenantEmailAvailability(
            cognitoId: request.CognitoId,
            email: request.Email
            );

        if (tenantIsNotAvailable)
        {
            return Result.Conflict(TenantExceptionsMessages.TenantNotAvailable);
        }

        var tenant = new Domain.Tenant(
            cognitoId: request.CognitoId,
            email: request.Email,
            name: request.Name,
            phoneNumber: request.PhoneNumber
            );

        await _tenantRepository.Create(tenant);
        
        var result = _mapper.Map(tenant);

        await _bus.Publish(new TenantCreated
        {
            TenantId = Guid.Parse(result.CognitoId),
            Email = result.Email,
            Name = result.Name,
            PhoneNumber = result.PhoneNumber
        }, cancellationToken);
        
        return Result.Success(result);
    }
}