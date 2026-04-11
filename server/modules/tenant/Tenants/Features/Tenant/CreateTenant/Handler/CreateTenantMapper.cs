using Riok.Mapperly.Abstractions;
using Tenants.Contracts;

namespace Tenants.Features.Tenant.CreateTenant.Handler;

[Mapper]
public partial class CreateTenantMapper
{
    [MapperIgnoreSource(nameof(Domain.Tenant.Id))]
    [MapperIgnoreSource(nameof(Domain.Tenant.FavoriteProperties))]
    [MapperIgnoreSource(nameof(Domain.Tenant.OwnedProperties))]
    public partial CreateTenantResponseDto Map(Domain.Tenant tenant);
}