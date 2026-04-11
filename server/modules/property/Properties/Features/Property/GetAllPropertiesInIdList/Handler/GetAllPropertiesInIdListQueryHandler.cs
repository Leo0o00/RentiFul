using Amazon.S3;
using Amazon.S3.Model;
using Ardalis.Result;
using Mediator;
using Microsoft.Extensions.Options;
using Properties.Contracts;
using Properties.Data.Repositories;
using Properties.Features.Manager.GetManagerProperties.Handler;
using Properties.Features.Property.GetAllPropertiesPaged.Handler;

namespace Properties.Features.Property.GetAllPropertiesInIdList.Handler;

public class GetAllPropertiesInIdListQueryHandler : IRequestHandler<GetAllPropertiesInIdListQuery, Result<PropertiesListDto>>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IAmazonS3 _s3Client;
    private readonly IOptions<S3Settings> _s3Settings;

    public GetAllPropertiesInIdListQueryHandler(IPropertyRepository propertyRepository, IOptions<S3Settings> s3Settings, IAmazonS3 s3Client)
    {
        _propertyRepository = propertyRepository;
        _s3Settings = s3Settings;
        _s3Client = s3Client;
    }
    
    public async ValueTask<Result<PropertiesListDto>> Handle(GetAllPropertiesInIdListQuery request, CancellationToken cancellationToken)
    {

        var guidList = request.PropertiesId.Select(id => Guid.Parse(id)).ToList();

        var result = await _propertyRepository.GetAllPropertiesInAnIdList(guidList);

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
                ManagerId = property.ManagerId
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