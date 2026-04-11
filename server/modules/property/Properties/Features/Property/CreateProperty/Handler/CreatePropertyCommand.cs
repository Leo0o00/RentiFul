using Ardalis.Result;
using FluentValidation;
using Mediator;
using Microsoft.AspNetCore.Http;
using Properties.Contracts;

namespace Properties.Features.Property.CreateProperty.Handler;

public class CreatePropertyCommand : IRequest<Result<CreatePropertyResponseDto>>
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

public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
{
    public CreatePropertyCommandValidator()
    {

    }
}