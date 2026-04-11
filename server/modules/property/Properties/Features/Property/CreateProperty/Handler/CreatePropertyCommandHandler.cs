using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;
using Ardalis.Result;
using MassTransit;
using Mediator;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Properties.Contracts;
using Properties.Data.Repositories;
using Properties.Domain;
using SharedContracts.Events;

namespace Properties.Features.Property.CreateProperty.Handler;

public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, Result<CreatePropertyResponseDto>>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IAmazonS3 _s3Client;
    private readonly IOptions<S3Settings> _s3Settings;
    private readonly IBus _bus;

    public CreatePropertyCommandHandler(IPropertyRepository propertyRepository, IHttpClientFactory httpClientFactory, IAmazonS3 s3Client, IOptions<S3Settings> s3Settings, IBus bus)
    {
        _propertyRepository = propertyRepository;
        _httpClientFactory = httpClientFactory;
        _s3Client = s3Client;
        _s3Settings = s3Settings;
        _bus = bus;
    }

    public async ValueTask<Result<CreatePropertyResponseDto>> Handle(CreatePropertyCommand request, CancellationToken ct)
    {
        // Check Manager Existence
        var manager = await _propertyRepository.GetManager(request.ManagerCognitoId);

        if (manager is null)
        {
            return Result.Error("Manager doesn't exist with the provided id.");
        }

        // Convert enums
        var amenities = new List<Amenity>();

        foreach (var amenity in request.Amenities)
        {
            amenities.Add(Enum.Parse<Amenity>(amenity));
        }

        var highlights = new List<Highlight>();

        foreach (var highlight in request.Highlights)
        {
            highlights.Add(Enum.Parse<Highlight>(highlight));
        }

        var propertyType = Enum.Parse<PropertyType>(request.PropertyType);

        // Create Location
        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"https://nominatim.openstreetmap.org/search?street={request.Address}&city={request.City}&country={request.Country}&postalcode={request.PostalCode}&format=json&limit=1")
        {
            Headers =
            {
                { HeaderNames.UserAgent, "RealEstateApp (justsomedummyemail@gmail.com)" }
            }
        };

        var httpClient = _httpClientFactory.CreateClient();
        var geocodingResponseMessage = await httpClient.SendAsync(
            request: httpRequestMessage,
            cancellationToken: ct
            );

        if (!geocodingResponseMessage.IsSuccessStatusCode)
        {
            return Result.Error(geocodingResponseMessage.ReasonPhrase);
        }

        await using var contentStream = await geocodingResponseMessage.Content.ReadAsStreamAsync(cancellationToken: ct);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var geocodingResponse = await JsonSerializer.DeserializeAsync
            <List<OpenStreetMapGetLocationResponse>>(
                contentStream,
                options: options,
                cancellationToken: ct
                );

        if (geocodingResponse is null || geocodingResponse.Count == 0)
        {
            return Result.Error("No geocoding response found.");
        }

        var location = new Location(
            address: request.Address,
            city: request.City,
            state: request.State,
            country: request.Country,
            postalCode: request.PostalCode,
            latitude: geocodingResponse[0].Latitude,
            longitude: geocodingResponse[0].Longitude
            );

        // Create Property
        var propertyToCreate = new Domain.Property(
            name: request.Name,
            description: request.Description,
            pricePerMonth: request.PricePerMonth,
            securityDeposit: request.SecurityDeposit,
            applicationFee: request.ApplicationFee,
            amenities: amenities,
            highlights: highlights,
            propertyType: propertyType,
            isPetsAllowed: request.IsPetsAllowed,
            isParkingIncluded: request.IsParkingIncluded,
            beds: request.Beds,
            baths: request.Baths,
            squareFeet: request.SquareFeet,
            propertyLocation: location,
            managerAssigned: manager);

        // Upload photos to S3
        foreach (var photo in request.Photos)
        {
            await using var stream = photo.OpenReadStream();

            var key = Guid.NewGuid();
            var putRequest = new PutObjectRequest
            {
                BucketName = _s3Settings.Value.BucketName,
                Key = $"images/{Constants.ModuleName}/{key}",
                InputStream = stream,
                ContentType = photo.ContentType,
                Metadata =
                {
                    ["file-name"] = photo.FileName
                }

            };

            await _s3Client.PutObjectAsync(putRequest, cancellationToken: ct);

            propertyToCreate.AddPhotoKey(key.ToString());


        }

        var propertyCreated = await _propertyRepository.CreateProperty(propertyToCreate);

        var response = new CreatePropertyResponseDto
        {
            PropertyId = propertyCreated
        };

        await _bus.Publish(new PropertyCreated
        {
            Id = response.PropertyId,
            ManagerId = manager.Id,
            PricePerMonth = request.PricePerMonth,
            SecurityDeposit = request.SecurityDeposit
        }, cancellationToken: ct);

        return response;
    }
}