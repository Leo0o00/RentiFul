using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Properties.Domain;
using SharedKernel;

namespace Properties.Features.Property.CreateProperty.Endpoint;

public class CreatePropertyRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal PricePerMonth { get; set; }
    public decimal SecurityDeposit { get; set; }
    public decimal ApplicationFee { get; set; }
    public bool IsPetsAllowed { get; set; }
    public bool IsParkingIncluded { get; set; }
    public string[] Amenities { get; set; }
    public string[] Highlights { get; set; }
    public int Beds { get; set; }
    public double Baths { get; set; }
    public double SquareFeet { get; set; }
    public string PropertyType { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string PostalCode { get; set; }
    public string ManagerCognitoId { get; set; }

    public IFormFileCollection Photos { get; set; }
}

public class CreatePropertyRequestValidator : Validator<CreatePropertyRequest>
{
    public CreatePropertyRequestValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(p => p.PricePerMonth)
            .NotEmpty().WithMessage("Price per Month is required.")
            .GreaterThan(0).WithMessage("Price per Month must be greater than 0.");

        RuleFor(p => p.SecurityDeposit)
            .NotEmpty().WithMessage("Security deposit is required.")
            .GreaterThan(0).WithMessage("Security deposit must be greater than 0.");

        RuleFor(p => p.ApplicationFee)
            .NotEmpty().WithMessage("Application fee is required.")
            .GreaterThan(0).WithMessage("Application fee must be greater than 0.");

        RuleFor(p => p.IsPetsAllowed)
            .NotEmpty().WithMessage("IsPetsAllowed is required.");

        RuleFor(p => p.IsParkingIncluded)
            .NotEmpty().WithMessage("IsParkingIncluded is required.");

        RuleFor(p => p.Amenities)
            .NotEmpty().WithMessage("Amenities is required.")
            .MustBeUniqueNormalized();

        RuleForEach(p => p.Amenities)
            .NotEmpty().WithMessage("Amenity {CollectionIndex} cannot be null or whitespace.")
            .MustBeEnumName<CreatePropertyRequest, Amenity>();

        RuleFor(p => p.Highlights)
            .NotEmpty().WithMessage("Highlights is required.")
            .MustBeUniqueNormalized();

        RuleForEach(p => p.Highlights)
            .NotEmpty().WithMessage("Highlight {CollectionIndex} cannot be null or whitespace.")
            .MustBeEnumName<CreatePropertyRequest, Highlight>();

        RuleFor(p => p.Beds)
            .NotEmpty().WithMessage("Beds is required.")
            .GreaterThan(0).WithMessage("Beds must be greater than 0.");

        RuleFor(p => p.Baths)
            .NotEmpty().WithMessage("Baths is required.")
            .GreaterThan(0).WithMessage("Baths must be greater than 0.");

        RuleFor(p => p.SquareFeet)
            .NotEmpty().WithMessage("SquareFeet is required.")
            .GreaterThan(0).WithMessage("SquareFeet must be greater than 0.");

        RuleFor(p => p.PropertyType)
            .NotEmpty().WithMessage("PropertyType is required.")
            .MustBeEnumName<CreatePropertyRequest, PropertyType>();

        RuleFor(p => p.Address)
            .NotEmpty().WithMessage("Address is required.");

        RuleFor(p => p.City)
            .NotEmpty().WithMessage("City is required.");

        RuleFor(p => p.State)
            .NotEmpty().WithMessage("State is required.");

        RuleFor(p => p.Country)
            .NotEmpty().WithMessage("Country is required.");

        RuleFor(p => p.PostalCode)
            .NotEmpty().WithMessage("PostalCode is required.");

        RuleFor(p => p.ManagerCognitoId)
            .NotEmpty().WithMessage("ManagerCognitoId is required.")
            .MustBeUuid();

        RuleFor(p => p.Photos)
            .NotEmpty().WithMessage("At least one photo is required.")
            .Must(x => x.Count <= 50).WithMessage("You can upload up to 50 photos at once.")
            .ForEach(photoRule =>
            {
                photoRule
                    .Must(x => IsAllowedSize(x.Length)).WithMessage("Photo {CollectionIndex} lenght must be between 10kB and 100kB.")
                    .Must(x => IsAllowedType(x.ContentType)).WithMessage("Photo {CollectionIndex} type is invalid!");
            });

    }

    private bool IsAllowedType(string contentType)
        => (new[] { "image/jpeg", "image/png", "image/webp" }).Contains(contentType.ToLower());

    private bool IsAllowedSize(long fileLength)
        => fileLength is >= 10000 and <= 500000;
}