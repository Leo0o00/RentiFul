using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Leases.Data.Repositories;
using SharedContracts;

namespace Leases.Features;

public class LeaseGrpcService: LeaseQueries.LeaseQueriesBase
{
    private readonly ILeaseRepository _leaseRepository;

    public LeaseGrpcService(ILeaseRepository leaseRepository)
    {
        _leaseRepository = leaseRepository;
    }

    public override async Task<GetLeaseInfoReply> GetLeaseInfo(GetLeaseInfoRequest request, ServerCallContext context)
    {
        var leaseId = Guid.Parse(request.LeaseId);

        var result = await _leaseRepository.GetLeaseById(leaseId);

        if (result is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Lease not found"));
        }

        return new GetLeaseInfoReply
        {
            Id = result.Id.ToString(),
            StartDate = result.StartDate.ToTimestamp(),
            EndDate = result.EndDate.ToTimestamp()
        };
    }
}