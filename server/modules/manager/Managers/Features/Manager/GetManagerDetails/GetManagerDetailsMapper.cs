using Managers.Contracts;
using Riok.Mapperly.Abstractions;

namespace Managers.Features.Manager.GetManagerDetails;

[Mapper]
public partial class GetManagerDetailsMapper
{
    [MapperIgnoreSource(nameof(Domain.Manager.Id))]
    public partial ManagerDetailsDto Map(Domain.Manager manager);
}