using System.Globalization;
using Amazon.S3;
using Amazon.S3.Model;
using Grpc.Core;
using Mediator;
using Microsoft.Extensions.Options;
using Properties.Data.Repositories;
using SharedContracts;
using LocationType = SharedContracts.LocationType;

namespace Properties.Features;

public class PropertyGrpcService: PropertyQueries.PropertyQueriesBase
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IAmazonS3 _s3Client;
    private readonly IOptions<S3Settings> _s3Settings;

    public PropertyGrpcService(IPropertyRepository propertyRepository, IOptions<S3Settings> s3Settings, IAmazonS3 s3Client)
    {
        _propertyRepository = propertyRepository;
        _s3Settings = s3Settings;
        _s3Client = s3Client;
    }

    public override async Task<GetPropertyInfoReply> GetPropertyInfo(GetPropertyInfoRequest request, ServerCallContext context)
    {
        var propertyId = Guid.Parse(request.PropertyId);

        var result = await _propertyRepository.GetPropertyInfoById(propertyId);

        if (result is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Property not found"));
        }

        var preSignedUrlRequest = new GetPreSignedUrlRequest
        {
            BucketName = _s3Settings.Value.BucketName,
            Key = $"images/{Constants.ModuleName}/{result.PhotoKey}",
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddMinutes(15)
        };
        string preSignedUrl = await _s3Client.GetPreSignedURLAsync(preSignedUrlRequest);

        return new GetPropertyInfoReply
        {
            Id = result.Id.ToString(),
            Name = result.Name,
            PricePerMonth = result.PricePerMonth.ToString(CultureInfo.CurrentCulture),
            PhotoUrl = preSignedUrl,
            Location = new LocationType
            {
                City = result.Location.City,
                Country = result.Location.Country,
            }
        };
    }
}