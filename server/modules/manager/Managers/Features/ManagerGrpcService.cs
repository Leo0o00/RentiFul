using Grpc.Core;
using Managers.Data.Repositories;
using SharedContracts;

namespace Managers.Features;

public class ManagerGrpcService: ManagerQueries.ManagerQueriesBase
{
    private readonly IManagerRepository _managerRepository;

    public ManagerGrpcService(IManagerRepository managerRepository)
    {
        _managerRepository = managerRepository;
    }

    public override async Task<GetManagerInfoReply> GetManagerInfo(GetManagerInfoRequest request, ServerCallContext context)
    {
        var managerCognitoId = Guid.Parse(request.ManagerCognitoId);

        var result = await _managerRepository.GetDetailsByCognitoId(managerCognitoId.ToString());

        if (result is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Manager not found"));
        }

        return new GetManagerInfoReply
        {
            ManagerCognitoId = result.CognitoId,
            Name = result.Name,
            Email = result.Email,
            PhoneNumber = result.PhoneNumber
        };
    }
}