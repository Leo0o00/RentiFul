using Grpc.Core;
using SharedContracts;
using Tenants.Data.Repositories;

namespace Tenants.Features;

public class TenantGrpcService: TenantQueries.TenantQueriesBase
{
    private readonly ITenantRepository _tenantRepository;

    public TenantGrpcService(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public override async Task<GetTenantInfoReply> GetTenantInfo(GetTenantInfoRequest request, ServerCallContext context)
    {
        var tenantCognitoId = Guid.Parse(request.TenantCognitoId);

        var result = await _tenantRepository.GetByCognitoId(tenantCognitoId.ToString());

        if (result is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Tenant not found"));
        }

        return new GetTenantInfoReply
        {
            TenantCognitoId = result.CognitoId,
            Email = result.Email,
            Name = result.Name,
            PhoneNumber = result.PhoneNumber
        };
    }
}