using Ardalis.Result;
using Managers.Contracts;
using Managers.Data.Repositories;
using Managers.ExceptionMessages;
using Managers.Features.Manager.CreateManager.Handler;
using Mediator;

namespace Managers.Features.Manager.GetManagerDetails.Handler;

public class GetManagerDetailsQueryHandler : IRequestHandler<GetManagerDetailsQuery, Result<ManagerDetailsDto>>
{
    private readonly IManagerRepository _managerRepository;
    private readonly GetManagerDetailsMapper _mapper;

    public GetManagerDetailsQueryHandler(IManagerRepository managerRepository, GetManagerDetailsMapper mapper)
    {
        _managerRepository = managerRepository;
        _mapper = mapper;
    }
    
    public async ValueTask<Result<ManagerDetailsDto>> Handle(GetManagerDetailsQuery request, CancellationToken cancellationToken)
    {
        var manager = await _managerRepository.GetDetailsByCognitoId(cognitoId: request.CognitoId);
        if (manager == null)
        {
            return Result.NotFound(ManagerExceptionsMessages.ManagerNotFound);
        }

        var result = _mapper.Map(manager);
        

        return Result.Success(result);
    }
}