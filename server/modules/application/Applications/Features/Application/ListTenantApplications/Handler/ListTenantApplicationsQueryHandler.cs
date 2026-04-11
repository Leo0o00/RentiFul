using System.Collections.Concurrent;
using Applications.Contracts;
using Applications.Data.Repositories;
using Applications.Features.Application.ListManagerApplications.Handler;
using Applications.Grpc;
using Ardalis.Result;
using Mediator;
using Serilog;

namespace Applications.Features.Application.ListTenantApplications.Handler;

public class ListTenantApplicationsQueryHandler(
    ILogger logger,
    IApplicationRepository applicationRepository,
    IPropertyGrpcServiceClient propertyGrpcClient,
    ILeaseGrpcServiceClient  leaseGrpcClient,
    ITenantGrpcServiceClient  tenantGrpcClient,
    IManagerGrpcServiceClient  managerGrpcClient)
    : IRequestHandler<ListTenantApplicationsQuery, Result<ListApplicationsResponseDto>>
{

    public async ValueTask<Result<ListApplicationsResponseDto>> Handle(ListTenantApplicationsQuery request, CancellationToken cancellationToken)
    {
        var applications = await applicationRepository.ListTenantApplications(tenantCognitoId: request.UserId, page: request.Page, limit: request.Limit);

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

            var managerId = application.ManagerId;

            if (managerId != null)
            {
                logger.Information("Fetching manager info. Id:'{ManagerId}'.", managerId);
                var manager = await managerGrpcClient.GetManagerInfo(managerCognitoId: managerId, ct: cancellationToken);
                if (manager is null)
                {
                    logger.Error("Manager with Id: {ManagerId} not found.", managerId);
                }
                else
                {
                    logger.Information("Manager with Id: {ManagerId} found.", managerId);
                    responseElement.Manager = manager;
                }

            }


            result.Add(responseElement);
        });

        return new ListApplicationsResponseDto(Count: applications.Count, Applications: result.ToList());

    }
}