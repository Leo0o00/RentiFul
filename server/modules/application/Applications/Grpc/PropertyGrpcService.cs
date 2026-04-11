using Applications.Contracts;
using SharedContracts;

namespace Applications.Grpc;

public interface IPropertyGrpcServiceClient
{
    Task<PropertyInfoDto?> GetPropertyInfo(Guid propertyId, CancellationToken ct);
}

public class PropertyGrpcServiceClient: IPropertyGrpcServiceClient
{
    private readonly PropertyQueries.PropertyQueriesClient _grpcClient;

    public PropertyGrpcServiceClient(PropertyQueries.PropertyQueriesClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    public async Task<PropertyInfoDto?> GetPropertyInfo(Guid propertyId, CancellationToken ct)
    {
        try
        {
            var response =
                await _grpcClient.GetPropertyInfoAsync(new GetPropertyInfoRequest { PropertyId = propertyId.ToString() },
                    cancellationToken: ct);

            return new PropertyInfoDto
            {
                Id = Guid.Parse(response.Id),
                Name = response.Name,
                PhotoUrl = response.PhotoUrl,
                PricePerMonth = Convert.ToDecimal(response.PricePerMonth),
                Location = new LocationDto
                {
                    City = response.Location.City,
                    Country = response.Location.Country,
                }
            };

        }
        catch (Exception e)
        {
            return null;
        }
    }
}