using Applications.Contracts;
using Applications.Data.Repositories;
using Applications.Domain;
using Ardalis.Result;
using Mediator;

namespace Applications.Features.Application.CreateApplication.Handler;

public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, Result<CreateApplicationResponseDto>>
{
    private readonly IApplicationRepository _applicationRepository;

    public CreateApplicationCommandHandler(IApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public async ValueTask<Result<CreateApplicationResponseDto>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var propertyId = Guid.Parse(request.PropertyId);
        var property = await _applicationRepository.GetProperty(propertyId);
        if (property is null)
        {
            return Result.NotFound("Property not found with the provided Id");
        }

        var tenantCognitoId = Guid.Parse(request.TenantCognitoId);
        var tenant = await _applicationRepository.GetTenant(tenantCognitoId);
        if (tenant is null)
        {
            return Result.NotFound("Tenant not found with the provided Id");
        }

        var applicationDate = DateTime.Parse(request.ApplicationDate).ToUniversalTime();
        var applicationStatus = Enum.Parse<ApplicationStatus>(request.Status);

        var applicationToCreate = new Domain.Application(applicationDate, applicationStatus, property, tenant, message: request.Message);

        var result = await _applicationRepository.CreateApplication(applicationToCreate);

        return new CreateApplicationResponseDto(result);
    }
}