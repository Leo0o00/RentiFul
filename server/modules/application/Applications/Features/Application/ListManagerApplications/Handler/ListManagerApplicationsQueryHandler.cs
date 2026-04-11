using System.Collections.Concurrent;
using Applications.Contracts;
using Applications.Data.Repositories;
using Applications.Grpc;
using Ardalis.Result;
using Mediator;
using Serilog;

namespace Applications.Features.Application.ListManagerApplications.Handler;

public class ListManagerApplicationsQueryHandler(
    ILogger logger,
    IApplicationRepository applicationRepository,
    IPropertyGrpcServiceClient propertyGrpcClient,
    ILeaseGrpcServiceClient  leaseGrpcClient,
    ITenantGrpcServiceClient  tenantGrpcClient,
    IManagerGrpcServiceClient  managerGrpcClient)
    : IRequestHandler<ListManagerApplicationsQuery, Result<ListApplicationsResponseDto>>
{

    public async ValueTask<Result<ListApplicationsResponseDto>> Handle(ListManagerApplicationsQuery request, CancellationToken cancellationToken)
    {
        var applications = await applicationRepository.ListManagerApplications(managerCognitoId: request.UserId, page: request.Page, limit: request.Limit);

        var result = new ConcurrentBag<ApplicationReponseDto>();

        // Todo: Implementar programacion dinamica aqui
        await Parallel.ForEachAsync(applications.Applications, cancellationToken, async (application, cancellationToken) =>
        {
            var responseElement = new ApplicationReponseDto
            {
                Id = application.Id,
                SubmitedAt = application.SubmitedAt,
                Status = application.Status
            };

            logger.Information("Fetching property info. Id:'{PropertyId}'.", application.PropertyId);
            var property = await propertyGrpcClient.GetPropertyInfo(propertyId: application.PropertyId, ct: cancellationToken);
            if (property is null)
            {
                logger.Error("Property with Id: {PropertyId} not found.", application.PropertyId);
            }
            else
            {
                logger.Information("Property with Id: {PropertyId} found.", application.PropertyId);
                responseElement.Property = property;

            }


            var leaseId = application.LeaseId;
            if (leaseId != null)
            {
                logger.Information("Fetching lease info. Id:'{LeaseId}'.", application.LeaseId);
                var lease = await leaseGrpcClient.GetLeaseInfo(leaseId: leaseId, ct: cancellationToken);

                if (lease is null)
                {
                    logger.Error("Lease with Id: {LeaseId} not found.", application.LeaseId);
                }
                else
                {
                    logger.Information("Lease with Id: {LeaseId} found.", application.LeaseId);
                    responseElement.Lease = lease;

                }
            }

            logger.Information("Fetching tenant info. Id:'{TenantId}'.", application.TenantCognitoId);
            var tenant = await tenantGrpcClient.GetTenantInfo(tenantCognitoId: application.TenantCognitoId, ct: cancellationToken);
            if (tenant is null)
            {
                logger.Error("Tenant with Id: {TenantId} not found.", application.TenantCognitoId);
            }
            else
            {
                logger.Information("Tenant with Id: {TenantId} found.", application.TenantCognitoId);
                responseElement.Tenant = tenant;
            }

            result.Add(responseElement);
        });

        return new ListApplicationsResponseDto(Count: applications.Count, Applications: result.ToList());

    }
}