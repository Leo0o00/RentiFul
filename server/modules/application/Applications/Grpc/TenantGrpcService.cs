using Applications.Contracts;
using SharedContracts;

namespace Applications.Grpc;

public interface ITenantGrpcServiceClient
{
    Task<TenantInfoDto?> GetTenantInfo(Guid tenantCognitoId, CancellationToken ct);

}

public class TenantGrpcServiceClient : ITenantGrpcServiceClient
{
    private readonly TenantQueries.TenantQueriesClient _grpcClient;

    public TenantGrpcServiceClient(TenantQueries.TenantQueriesClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    public async Task<TenantInfoDto?> GetTenantInfo(Guid tenantCognitoId, CancellationToken ct)
    {
        try
        {
            var response = await _grpcClient.GetTenantInfoAsync(new GetTenantInfoRequest() { TenantCognitoId = tenantCognitoId.ToString() },
                cancellationToken: ct);

            return new TenantInfoDto()
            {
                CongitoId = Guid.Parse(response.TenantCognitoId),
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