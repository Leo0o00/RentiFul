using Amazon.S3;
using Amazon.S3.Model;
using Ardalis.Result;
using Mediator;
using Microsoft.Extensions.Options;
using Properties.Contracts;
using Properties.Data.Repositories;

namespace Properties.Features.Property.GetPropertyDetails.Handler;

public class GetPropertyDetailsQueryHandler : IRequestHandler<GetPropertyDetailsQuery, Result<PropertyDetailsDto>>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IAmazonS3 _s3Client;
    private readonly IOptions<S3Settings> _s3Settings;

    public GetPropertyDetailsQueryHandler(IPropertyRepository propertyRepository, IOptions<S3Settings> s3Settings, IAmazonS3 s3Client)
    {
        _propertyRepository = propertyRepository;
        _s3Settings = s3Settings;
        _s3Client = s3Client;
    }

    public async ValueTask<Result<PropertyDetailsDto>> Handle(GetPropertyDetailsQuery request, CancellationToken ct)
    {
        var property = await _propertyRepository.GetPropertyById(request.PropertyId);

        if (property is null)
        {
            return Result.NotFound();
        }

        var urls = new List<string>();

        foreach (var column in property.PhotoKeys)
        {
            var preSignedUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = _s3Settings.Value.BucketName,
                Key = $"images/{Constants.ModuleName}/{column.PhotoKey}",
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddMinutes(15)
            };

            var preSignedUrl = await _s3Client.GetPreSignedURLAsync(preSignedUrlRequest);
            urls.Add(preSignedUrl);
        }

        var amenities = property.Amenities
            .Select(a => a.ToString()).ToList();

        var highlights = property.Highlights
            .Select(a => a.ToString()).ToList();

        var coordinates = new PropertyDetailsLocationCoordinatesDto
        {
            Longitude = property.PropertyLocation.Coordinates.X,
            Latitude = property.PropertyLocation.Coordinates.Y,
        };

        var location = new PropertyDetailsLocationDto
        {
            Address = property.PropertyLocation.Address,
            City = property.PropertyLocation.City,
            Country = property.PropertyLocation.Country,
            State = property.PropertyLocation.State,
            Coordinates = coordinates
        };

        var response = new PropertyDetailsDto
        {
            Id = property.Id,
            Name = property.Name,
            Description = property.Description,
            PricePerMonth = property.PricePerMonth,
            SecurityDeposit = property.SecurityDeposit,
            ApplicationFee = property.ApplicationFee,
            Amenities = amenities,
            Highlights = highlights,
            IsPetsAllowed = property.IsPetsAllowed,
            IsParkingIncluded = property.IsParkingIncluded,
            Beds = property.Beds,
            Baths = property.Baths,
            SquareFeet = property.SquareFeet,
            AverageRating = property.AverageRating,
            NumberOfReviews = property.NumberOfReviews,
            Location = location,
            PhotoUrls = urls.ToArray(),
            ManagerId = property.ManagerId

        };

        return response;

    }
}