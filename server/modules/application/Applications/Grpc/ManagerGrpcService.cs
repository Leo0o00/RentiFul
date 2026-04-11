using Applications.Contracts;
using SharedContracts;

namespace Applications.Grpc;

public interface IManagerGrpcServiceClient
{
    Task<ManagerInfoDto?> GetManagerInfo(Guid? managerCognitoId, CancellationToken ct);

}

public class ManagerGrpcServiceClient : IManagerGrpcServiceClient
{
    private readonly ManagerQueries.ManagerQueriesClient _grpcClient;

    public ManagerGrpcServiceClient(ManagerQueries.ManagerQueriesClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    public async Task<ManagerInfoDto?> GetManagerInfo(Guid? managerCognitoId, CancellationToken ct)
    {
        try
        {
            var response = await _grpcClient.GetManagerInfoAsync(new GetManagerInfoRequest() { ManagerCognitoId = managerCognitoId.ToString() },
                cancellationToken: ct);

            return new ManagerInfoDto()
            {
                CongitoId = Guid.Parse(response.ManagerCognitoId),
                Name = response.Name,
                PhoneNumber = response.PhoneNumber,
                Email = response.Email
            };

        }
        catch (Exception e)
        {
            return null;
        }
    }
}