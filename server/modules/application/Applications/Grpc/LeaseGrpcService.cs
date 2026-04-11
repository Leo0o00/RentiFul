using Applications.Contracts;
using SharedContracts;

namespace Applications.Grpc;

public interface ILeaseGrpcServiceClient
{
    Task<LeaseInfoDto?> GetLeaseInfo(Guid? leaseId, CancellationToken ct);
}

public class LeaseGrpcServiceClient: ILeaseGrpcServiceClient
{
    private readonly LeaseQueries.LeaseQueriesClient _grpcClient;

    public LeaseGrpcServiceClient(LeaseQueries.LeaseQueriesClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    public async Task<LeaseInfoDto?> GetLeaseInfo(Guid? leaseId, CancellationToken ct)
    {
        try
        {
            var response = await _grpcClient.GetLeaseInfoAsync(new GetLeaseInfoRequest { LeaseId = leaseId.ToString() },
                cancellationToken: ct);

            return new LeaseInfoDto
            {
                Id = Guid.Parse(response.Id),
                StartDate = response.StartDate.ToDateTime(),
                EndDate = response.EndDate.ToDateTime()
            };

        }
        catch (Exception e)
        {
            return null;
        }
    }

}