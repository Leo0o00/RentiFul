namespace Applications.Contracts;

public class PropertyInfoDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal PricePerMonth { get; set; }
    public string PhotoUrl { get; set; }
    public LocationDto Location { get; set; }
}

public class LocationDto
{
    public string City { get; set; }
    public string Country { get; set; }
}