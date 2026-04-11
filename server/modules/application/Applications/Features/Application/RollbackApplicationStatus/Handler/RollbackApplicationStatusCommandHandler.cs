using Applications.Data.Repositories;
using Ardalis.Result;
using Mediator;

namespace Applications.Features.Application.RollbackApplicationStatus.Handler;

public class RollbackApplicationStatusCommandHandler(IApplicationRepository applicationRepository): IRequestHandler<RollbackApplicationStatusCommand, Result>
{
    public async ValueTask<Result> Handle(RollbackApplicationStatusCommand request, CancellationToken cancellationToken)
    {
        var application = await applicationRepository.GetApplication(request.ApplicationId, false);
        if (application is null)
        {
            return Result.NotFound("Application not found with the provided id");
        }

        application.RollbackApplicationStatus();

        await applicationRepository.SaveChangesAsync();

        return Result.Success();
    }
}