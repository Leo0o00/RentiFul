namespace Properties.Contracts;

public class PropertyResponseDto
{
    public Guid Id { get; set; }
    public string Name {get; set;}
    public string[]? PhotoUrls {get; set;}

    public bool IsPetsAllowed {get; set;}

    public bool IsParkingIncluded {get; set;}


    public LocationDto Location {get; set;}

    public double? AverageRating {get; set;}

    public int? NumberOfReviews {get; set;}

    public decimal PricePerMonth {get; set;}

    public int Beds {get; set;}

    public double Baths {get; set;}

    public double SquareFeet {get; set;}

    public Guid? ManagerId {get; set;}

}