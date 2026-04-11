using Properties.Features.Property.CreateProperty.Endpoint;
using Properties.Features.Property.CreateProperty.Handler;
using Riok.Mapperly.Abstractions;

namespace Properties.Features.Property.CreateProperty;

[Mapper]
public partial class CreatePropertyMapper
{
    public partial CreatePropertyCommand Map(CreatePropertyRequest request);

}