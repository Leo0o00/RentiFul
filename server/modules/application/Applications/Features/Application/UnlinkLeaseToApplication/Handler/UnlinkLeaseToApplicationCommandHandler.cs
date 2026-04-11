using Applications.Data.Repositories;
using Ardalis.Result;
using Mediator;

namespace Applications.Features.Application.UnlinkLeaseToApplication.Handler;
public class UnlinkLeaseToApplicationCommandHandler(IApplicationRepository applicationRepository)
    : IRequestHandler<UnlinkLeaseToApplicationCommand, Result>
{

    public async ValueTask<Result> Handle(UnlinkLeaseToApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await applicationRepository.GetApplication(applicationId: request.ApplicationId, noTracking: false);
        if (application is null)
        {
            return Result.NotFound("Application not found with the provided id.");
        }
        var lease = await applicationRepository.GetLease(leaseId: request.LeaseId);
        if (lease is null)
        {
            return Result.NotFound("Lease not found with the provided id.");
        }

        application.UnlinkLease();

        await applicationRepository.SaveChangesAsync();

        // await applicationRepository.RemoveLease(lease);


        return Result.Success();
    }
}