using Applications.Data.Repositories;
using Applications.Domain;
using Ardalis.Result;
using Mediator;

namespace Applications.Features.Application.LinkLeaseToApplication.Handler;

public class LinkLeaseToApplicationCommandHandler : IRequestHandler<LinkLeaseToApplicationCommand, Result<Guid>>
{
    private readonly IApplicationRepository _applicationRepository;

    public LinkLeaseToApplicationCommandHandler(IApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public async ValueTask<Result<Guid>> Handle(LinkLeaseToApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await _applicationRepository.GetApplication(applicationId: request.ApplicationId, noTracking: false);

        if (application is null)
        {
            return Result.NotFound("Application not found with the provided id");
        }

        application.AddLease(new Lease(leaseId: request.LeaseId));

        await _applicationRepository.SaveChangesAsync();

        return application.Id;
    }
}