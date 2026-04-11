using Applications.Contracts;
using Applications.Data.Repositories;
using Applications.Domain;
using Applications.Features.Application.CreateApplication.Handler;
using Ardalis.Result;
using MassTransit;
using Mediator;
using SharedContracts.Commands;
using SharedContracts.Events;

namespace Applications.Features.Application.UpdateApplication.Handler;

public class UpdateApplicationCommandHandler : IRequestHandler<UpdateApplicationCommand, Result>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IBus _bus;

    public UpdateApplicationCommandHandler(IApplicationRepository applicationRepository, IBus bus)
    {
        _applicationRepository = applicationRepository;
        _bus = bus;
    }

    public async ValueTask<Result> Handle(UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        var applicationId = Guid.Parse(request.ApplicationId);
        var incomingApplicationStatus = Enum.Parse<ApplicationStatus>(request.Status.Trim(), true);

        var application = await _applicationRepository.GetApplication(applicationId, noTracking: false);
        if (application is null)
        {
            return Result.NotFound("Application not found with the provided Id");
        }

        if (application.Status == incomingApplicationStatus)
        {
            return Result.Conflict();
        }

        if (application.Status is ApplicationStatus.Approved or ApplicationStatus.Denied)
        {
            return Result.Error("Application Status has already been Approved or Denied");
        }

        if (incomingApplicationStatus == ApplicationStatus.Approved)
        {
            application.UpdateApplicationStatus(applicationStatus: ApplicationStatus.Approved);

            await _applicationRepository.SaveChangesAsync();

            await _bus.Publish(new ApplicationApproved
            {
                ApplicationId = applicationId,
                ApprovedAt = DateTime.UtcNow,
                PropertyId = application.PropertyId,
                TenantCognitoId = application.TenantId
            }, cancellationToken);
        }
        else
        {
            application.UpdateApplicationStatus(applicationStatus: ApplicationStatus.Denied);

            await _applicationRepository.SaveChangesAsync();

        }


        return Result.Success();
    }
}