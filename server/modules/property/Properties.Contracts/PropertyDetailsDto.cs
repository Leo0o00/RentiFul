namespace Properties.Contracts;

public class PropertyDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal PricePerMonth { get; set; }
    public decimal SecurityDeposit { get; set; }
    public decimal ApplicationFee { get; set; }
    public List<string> Amenities { get; set; }
    public List<string>  Highlights { get; set; }
    public bool IsPetsAllowed { get; set; }
    public bool IsParkingIncluded { get; set; }
    public int Beds { get; set; }
    public double Baths { get; set; }
    public double SquareFeet { get; set; }
    public double? AverageRating { get; set; }
    public int? NumberOfReviews { get; set; }
    public PropertyDetailsLocationDto Location { get; set; }
    public string[]? PhotoUrls {get; set;}

    public Guid? ManagerId { get; set; }
}

public class PropertyDetailsLocationDto
{
    public string Address { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public PropertyDetailsLocationCoordinatesDto Coordinates { get; set; }

}

public class PropertyDetailsLocationCoordinatesDto
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}