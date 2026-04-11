using Amazon.S3;
using Amazon.S3.Model;
using Ardalis.Result;
using Mediator;
using Microsoft.Extensions.Options;
using Properties.Contracts;
using Properties.Data.Helpers;
using Properties.Data.Repositories;

namespace Properties.Features.Property.GetAllPropertiesPaged.Handler;

public class GetAllPropertiesPagedQueryHandler : IRequestHandler<GetAllPropertiesPagedQuery, Result<PropertiesListDto>>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IAmazonS3 _s3Client;
    private readonly IOptions<S3Settings> _s3Settings;
    private readonly GetAllPropertiesPagedMapper _mapper;

    public GetAllPropertiesPagedQueryHandler(IPropertyRepository propertyRepository, GetAllPropertiesPagedMapper mapper, IOptions<S3Settings> s3Settings, IAmazonS3 s3Client)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
        _s3Settings = s3Settings;
        _s3Client = s3Client;
    }
    
    public async ValueTask<Result<PropertiesListDto>> Handle(GetAllPropertiesPagedQuery request, CancellationToken cancellationToken)
    {
        var filters = new PropertiesQueryFilters(
            Location: request.Location,
            PriceMin: request.PriceMin,
            PriceMax: request.PriceMax,
            Beds: request.Beds,
            Baths: request.Baths,
            PropertyType: request.PropertyType,
            SquareFeetMin: request.SquareFeetMin,
            SquareFeetMax: request.SquareFeetMax,
            Amenities: request.Amenities,
            AvailableFrom: request.AvailableFrom,
            FavoriteIds: request.FavoriteIds,
            Latitude: request.Latitude,
            Longitude: request.Longitude
            );

        var result = await _propertyRepository.GetPropertiesByFilters(filters);

        var count = result.count;
        var propertiesList = new List<PropertyResponseDto>();



        foreach (PropertyDto property in result.properties)
        {
            var finalPropertyObject = new PropertyResponseDto
            {
                Id = property.Id,
                Name = property.Name,
                PricePerMonth = property.PricePerMonth,
                IsPetsAllowed = property.IsPetsAllowed,
                IsParkingIncluded = property.IsParkingIncluded,
                Beds = property.Beds,
                Baths = property.Baths,
                SquareFeet = property.SquareFeet,
                AverageRating = property.AverageRating,
                NumberOfReviews = property.NumberOfReviews,
                Location = property.Location,
                ManagerId = property.ManagerId,
            };

            var preSignedUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = _s3Settings.Value.BucketName,
                Key = $"images/{Constants.ModuleName}/{property.PhotoKey}",
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddMinutes(15)
            };

            var presignedUrlList = new List<string>();

            string preSignedUrl = await _s3Client.GetPreSignedURLAsync(preSignedUrlRequest);

            presignedUrlList.Add(preSignedUrl);

            finalPropertyObject.PhotoUrls = presignedUrlList.ToArray();

            propertiesList.Add(finalPropertyObject);
        }

        var response = new PropertiesListDto
        {
            count = count,
            properties = propertiesList.ToArray(),
        };

        return response;

    }
}